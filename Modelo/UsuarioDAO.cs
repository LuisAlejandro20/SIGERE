using System;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DataAccess
{
    public class UsuarioDAO
    {
        private readonly string connectionString = "Server=DESKTOP-SKD3024;Database=SIGERE_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        public Usuario AutenticarUsuario(string nombreUsuario, string passwordPlana)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El campo nombre está vacío");
            if (string.IsNullOrWhiteSpace(passwordPlana))
                throw new ArgumentException("El campo contraseña está vacío");

            Usuario usuarioAutenticado = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT idUsuario, NOMBRE, USUARIO, PASSWORD, ROL, CORREO_ELECTRONICO, TELEFONO, ESTATUS, FECHA_ALTA FROM Usuarios WHERE USUARIO = @usuario";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario", nombreUsuario);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string estatus = reader["ESTATUS"].ToString();
                            if (estatus != "Activo")
                            {
                                throw new InvalidOperationException("Su cuenta se encuentra inactiva. Contacte al administrador.");
                            }

                            string passwordCifradaBD = reader["PASSWORD"].ToString();

                            if (VerificarPasswordCifrada(passwordPlana, passwordCifradaBD))
                            {
                                usuarioAutenticado = new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                                    Nombre = reader["NOMBRE"].ToString(),
                                    NombreUsuario = reader["USUARIO"].ToString(),
                                    Password = passwordCifradaBD,
                                    Rol = reader["ROL"].ToString(),
                                    CorreoElectronico = reader["CORREO_ELECTRONICO"].ToString(),
                                    Telefono = reader["TELEFONO"] != DBNull.Value ? Convert.ToInt32(reader["TELEFONO"]) : 0,
                                    Estatus = estatus,
                                    FechaAlta = Convert.ToDateTime(reader["FECHA_ALTA"])
                                };
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return null;
                }
            }

            if (usuarioAutenticado != null && !usuarioAutenticado.EsInvarianteValido())
            {
                throw new InvalidOperationException("Invariante violado: El usuario autenticado no posee un estatus de cuenta válido.");
            }

            return usuarioAutenticado;
        }

        public bool VerificarPasswordCifrada(string passwordPlana, string passwordCifradaBD)
        {
            if (string.IsNullOrEmpty(passwordPlana) || string.IsNullOrEmpty(passwordCifradaBD))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(passwordPlana, passwordCifradaBD);
            }
            catch
            {
                return passwordPlana == passwordCifradaBD;
            }
        }

        public bool ValidarPermisoModulo(Usuario usuario, string moduloRequerido)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Precondición fallida: El usuario no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(moduloRequerido))
                throw new ArgumentException("Precondición fallida: Debe especificar el módulo solicitado.");

            if (usuario.Rol == "Gerente")
                return true;

            if (usuario.Rol == "Vendedor" && (moduloRequerido == "Cobro" || moduloRequerido == "Inventario"))
                return true;

            return false;
        }

        public bool ValidarTelefonoExistente(int telefono)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE TELEFONO = @telefono";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@telefono", telefono);

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ValidarCorreoExistente(string correo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE CORREO_ELECTRONICO = @correo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@correo", correo);

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ActualizarPasswordPorTelefono(int telefono, string nuevaPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET PASSWORD = @nuevaPass WHERE TELEFONO = @telefono";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nuevaPass", nuevaPassword);
                command.Parameters.AddWithValue("@telefono", telefono);

                try
                {
                    connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}