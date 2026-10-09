using System;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DataAccess
{
    public class UsuarioDAO
    {
        private readonly string connectionString = "Server=localhost;Database=SIGERE_DB;Trusted_Connection=True;";

        /// <summary>
        /// MÉTODO 1: Autenticar Usuario
        /// PRECONDICIÓN: nombreUsuario y passwordPlana no deben ser vacíos ni nulos.
        /// POSCONDICIÓN: Si las credenciales son válidas, garantiza un objeto Usuario válido; si no, retorna null.
        /// INVARIANTE: El usuario devuelto debe cumplir con un estatus válido.
        /// </summary>
        public Usuario AutenticarUsuario(string nombreUsuario, string passwordPlana)
        {
            // --- PRECONDICIONES ---
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Precondición fallida: El usuario no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(passwordPlana))
                throw new ArgumentException("Precondición fallida: La contraseña no puede estar vacía.");

            Usuario usuarioAutenticado = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT idUsuario, NOMBRE, USUARIO, PASSWORD, ROL, CORREO_ELECTRONICO, TELEFONO, ESTATUS, FECHA_ALTA FROM Usuarios WHERE USUARIO = @usuario AND ESTATUS = 'Activo'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario", nombreUsuario);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
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
                                    Estatus = reader["ESTATUS"].ToString(),
                                    FechaAlta = Convert.ToDateTime(reader["FECHA_ALTA"])
                                };
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    // Manejo para ejecución local sin servidor SQL activo
                    return null;
                }
            }

            // --- POSCONDICIÓN E INVARIANTE ---
            if (usuarioAutenticado != null && !usuarioAutenticado.EsInvarianteValido())
            {
                throw new InvalidOperationException("Invariante violado: El usuario autenticado no posee un estatus de cuenta válido.");
            }

            return usuarioAutenticado;
        }

        /// <summary>
        /// MÉTODO 2: Verificar Contraseña Cifrada
        /// PRECONDICIÓN: Las cadenas de contraseña plana y cifrada deben ser válidas.
        /// POSCONDICIÓN: Devuelve el resultado del hash BCrypt sin excepciones no controladas.
        /// </summary>
        public bool VerificarPasswordCifrada(string passwordPlana, string passwordCifradaBD)
        {
            // --- PRECONDICIÓN ---
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

        /// <summary>
        /// MÉTODO 3: Validar Permisos por Rol
        /// PRECONDICIÓN: El usuario no debe ser nulo y el módulo solicitado debe ser especificado.
        /// POSCONDICIÓN: Retorna true únicamente si el rol autoriza el módulo de forma explícita.
        /// </summary>
        public bool ValidarPermisoModulo(Usuario usuario, string moduloRequerido)
        {
            // --- PRECONDICIÓN ---
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Precondición fallida: El usuario no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(moduloRequerido))
                throw new ArgumentException("Precondición fallida: Debe especificar el módulo solicitado.");

            // --- REGLAS DE NEGOCIO ---
            if (usuario.Rol == "Gerente")
                return true;

            if (usuario.Rol == "Vendedor" && (moduloRequerido == "Cobro" || moduloRequerido == "Inventario"))
                return true;

            return false;
        }
    }
}