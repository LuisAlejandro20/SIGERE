using System;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DataAccess
{
    public class UsuarioDAO
    {
        private readonly string connectionString = "Server=myServerAddress;Database=SIGERE_DB;User Id=myUsername;Password=myPassword;";

        // Método 1: Autenticar Usuario en la base de datos
        public Usuario AutenticarUsuario(string nombreUsuario, string passwordPlana)
        {
            Usuario usuarioAutenticado = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Se busca al usuario por su nombre de usuario
                string query = "SELECT idUsuario, NOMBRE, USUARIO, PASSWORD, ROL, CORREO_ELECTRONICO, TELEFONO, ESTATUS, FECHA_ALTA FROM Usuarios WHERE USUARIO = @usuario AND ESTATUS = 'Activo'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario", nombreUsuario);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string passwordCifradaBD = reader["PASSWORD"].ToString();
                        
                        // Se verifica la contraseña cifrada
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
            return usuarioAutenticado;
        }

        // Método 2: Verificar contraseña cifrada
        public bool VerificarPasswordCifrada(string passwordPlana, string passwordCifradaBD)
        {
            // Lógica de validación usando un algoritmo de Hash (ej. BCrypt)
            // Se asume el uso de una librería de hashing para cumplir con RNF8
            return BCrypt.Net.BCrypt.Verify(passwordPlana, passwordCifradaBD);
        }

        // Método 3: Validar el permiso de acceso a módulos basado en el rol
        public bool ValidarPermisoModulo(Usuario usuario, string moduloRequerido)
        {
            if (usuario.Rol == "Gerente") 
                return true; 

            if (usuario.Rol == "Vendedor" && (moduloRequerido == "Cobro" || moduloRequerido == "Inventario"))
                return true; 

            return false;
        }
        //
    }
}