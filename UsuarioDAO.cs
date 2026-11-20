using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DAO
{
    public sealed class UsuarioDAO
    {
        private static readonly Lazy<UsuarioDAO> _instancia =
            new Lazy<UsuarioDAO>(() => new UsuarioDAO());

        private UsuarioDAO() { }

        public static UsuarioDAO Instancia => _instancia.Value;

        public int Crear(Usuario usuario, string passwordPlano)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(passwordPlano, workFactor: 12);

            const string query = @"
                INSERT INTO Usuarios (Nombre, Correo, PasswordHash, Rol, Activo)
                OUTPUT INSERTED.IdUsuario
                VALUES (@Nombre, @Correo, @PasswordHash, @Rol, @Activo);";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                comando.Parameters.AddWithValue("@Correo", usuario.Correo);
                comando.Parameters.AddWithValue("@PasswordHash", hash);
                comando.Parameters.AddWithValue("@Rol", usuario.Rol);
                comando.Parameters.AddWithValue("@Activo", usuario.Activo);

                conexion.Open();
                return (int)comando.ExecuteScalar();
            }
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            const string query = @"
                SELECT IdUsuario, Nombre, Correo, Rol, Activo, FechaCreacion
                FROM Usuarios
                ORDER BY Nombre ASC;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                conexion.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapearUsuarioSinHash(reader));
                    }
                }
            }
            return lista;
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            const string query = @"
                SELECT IdUsuario, Nombre, Correo, Rol, Activo, FechaCreacion
                FROM Usuarios
                WHERE IdUsuario = @IdUsuario;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conexion.Open();

                using (var reader = comando.ExecuteReader())
                {
                    return reader.Read() ? MapearUsuarioSinHash(reader) : null;
                }
            }
        }

        public Usuario Autenticar(string correo, string passwordPlano)
        {
            const string query = @"
                SELECT IdUsuario, Nombre, Correo, PasswordHash, Rol, Activo, FechaCreacion
                FROM Usuarios
                WHERE Correo = @Correo AND Activo = 1;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Correo", correo);
                conexion.Open();

                using (var reader = comando.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    string hashAlmacenado = reader["PasswordHash"].ToString();
                    if (!BCrypt.Net.BCrypt.Verify(passwordPlano, hashAlmacenado))
                        return null;

                    return MapearUsuarioSinHash(reader);
                }
            }
        }

        public bool Actualizar(Usuario usuario, string nuevaPasswordPlano = null)
        {
            bool actualizarPassword = !string.IsNullOrWhiteSpace(nuevaPasswordPlano);

            string query = actualizarPassword
                ? @"UPDATE Usuarios
                    SET Nombre = @Nombre, Correo = @Correo, PasswordHash = @PasswordHash, Rol = @Rol, Activo = @Activo
                    WHERE IdUsuario = @IdUsuario;"
                : @"UPDATE Usuarios
                    SET Nombre = @Nombre, Correo = @Correo, Rol = @Rol, Activo = @Activo
                    WHERE IdUsuario = @IdUsuario;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                comando.Parameters.AddWithValue("@Correo", usuario.Correo);
                comando.Parameters.AddWithValue("@Rol", usuario.Rol);
                comando.Parameters.AddWithValue("@Activo", usuario.Activo);

                if (actualizarPassword)
                {
                    string nuevoHash = BCrypt.Net.BCrypt.HashPassword(nuevaPasswordPlano, workFactor: 12);
                    comando.Parameters.AddWithValue("@PasswordHash", nuevoHash);
                }

                conexion.Open();
                return comando.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int idUsuario)
        {
            const string query = @"
                UPDATE Usuarios
                SET Activo = 0
                WHERE IdUsuario = @IdUsuario;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conexion.Open();
                return comando.ExecuteNonQuery() > 0;
            }
        }

        private static Usuario MapearUsuarioSinHash(SqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                Nombre = reader["Nombre"].ToString(),
                Correo = reader["Correo"].ToString(),
                Rol = reader["Rol"].ToString(),
                Activo = Convert.ToBoolean(reader["Activo"]),
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
            };
        }
    }
}