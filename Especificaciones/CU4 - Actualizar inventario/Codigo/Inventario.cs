using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DataAccess
{
    public class Inventario
    {
        // Misma cadena de conexión utilizada en UsuarioDAO.cs
        private readonly string connectionString = "Server=DESKTOP-SKD3024;Database=SIGERE_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Producto> BuscarDisponibilidad(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                throw new ArgumentException("Precondición fallida: El criterio de búsqueda no puede estar vacío.");

            List<Producto> resultados = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT p.IdProducto, p.NOMBRE, p.CLAVE, p.IdCategoria, c.NOMBRE AS NombreCategoria,
                           p.PRECIO, p.EXISTENCIA, p.STOCK_MINIMO, p.UBICACION, p.IdProveedor, p.ESTATUS
                    FROM Productos p
                    INNER JOIN Categorias c ON p.IdCategoria = c.IdCategoria
                    WHERE p.ESTATUS = 'Activo' 
                      AND (p.CLAVE = @criterioExacto OR p.NOMBRE LIKE @criterioParcial)
                    ORDER BY p.NOMBRE ASC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@criterioExacto", criterio.Trim());
                command.Parameters.AddWithValue("@criterioParcial", "%" + criterio.Trim() + "%");

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto prod = new Producto
                            {
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["NOMBRE"].ToString(),
                                Clave = reader["CLAVE"].ToString(),
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                NombreCategoria = reader["NombreCategoria"].ToString(),
                                Precio = Convert.ToDecimal(reader["PRECIO"]),
                                Existencia = Convert.ToInt32(reader["EXISTENCIA"]),
                                StockMinimo = Convert.ToInt32(reader["STOCK_MINIMO"]),
                                Ubicacion = reader["UBICACION"] != DBNull.Value ? reader["UBICACION"].ToString() : "Sin ubicación",
                                IdProveedor = reader["IdProveedor"] != DBNull.Value ? Convert.ToInt32(reader["IdProveedor"]) : (int?)null,
                                Estatus = reader["ESTATUS"].ToString()
                            };

                            if (prod.EsInvarianteValido())
                            {
                                resultados.Add(prod);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error al consultar la disponibilidad en la base de datos.", ex);
                }
            }

            return resultados;
        }

        public List<Producto> ObtenerCatalogoCompleto()
        {
            List<Producto> resultados = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT p.IdProducto, p.NOMBRE, p.CLAVE, p.IdCategoria, c.NOMBRE AS NombreCategoria,
                           p.PRECIO, p.EXISTENCIA, p.STOCK_MINIMO, p.UBICACION, p.IdProveedor, p.ESTATUS
                    FROM Productos p
                    INNER JOIN Categorias c ON p.IdCategoria = c.IdCategoria
                    WHERE p.ESTATUS = 'Activo'
                    ORDER BY p.NOMBRE ASC";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["NOMBRE"].ToString(),
                                Clave = reader["CLAVE"].ToString(),
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                NombreCategoria = reader["NombreCategoria"].ToString(),
                                Precio = Convert.ToDecimal(reader["PRECIO"]),
                                Existencia = Convert.ToInt32(reader["EXISTENCIA"]),
                                StockMinimo = Convert.ToInt32(reader["STOCK_MINIMO"]),
                                Ubicacion = reader["UBICACION"] != DBNull.Value ? reader["UBICACION"].ToString() : "Sin ubicación",
                                IdProveedor = reader["IdProveedor"] != DBNull.Value ? Convert.ToInt32(reader["IdProveedor"]) : (int?)null,
                                Estatus = reader["ESTATUS"].ToString()
                            });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error al cargar el catálogo de productos.", ex);
                }
            }

            return resultados;
        }
    }
}