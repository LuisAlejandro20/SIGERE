using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SIGERE.Models;

namespace SIGERE.DAO
{
    public sealed class OrdenCompraDAO
    {
        private static readonly Lazy<OrdenCompraDAO> _instancia =
            new Lazy<OrdenCompraDAO>(() => new OrdenCompraDAO());

        private OrdenCompraDAO() { }

        public static OrdenCompraDAO Instancia => _instancia.Value;

        public List<RefaccionStockCritico> ObtenerRefaccionesEnStockMinimo()
        {
            var criticas = new List<RefaccionStockCritico>();
            const string query = @"
                SELECT IdRefaccion, Codigo, Nombre, IdProveedor, StockActual, StockMinimo, StockMaximo, CostoCompra
                FROM Refacciones
                WHERE StockActual <= StockMinimo
                ORDER BY IdProveedor, StockActual ASC;";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            using (var comando = new SqlCommand(query, conexion))
            {
                conexion.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        criticas.Add(new RefaccionStockCritico
                        {
                            IdRefaccion = Convert.ToInt32(reader["IdRefaccion"]),
                            Codigo = reader["Codigo"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            StockActual = Convert.ToInt32(reader["StockActual"]),
                            StockMinimo = Convert.ToInt32(reader["StockMinimo"]),
                            StockMaximo = Convert.ToInt32(reader["StockMaximo"]),
                            CostoCompra = Convert.ToDecimal(reader["CostoCompra"])
                        });
                    }
                }
            }
            return criticas;
        }

        public int RegistrarOrdenConDetalles(OrdenCompra orden)
        {
            const string insertOrden = @"
                INSERT INTO OrdenesCompra (IdProveedor, IdUsuario, FechaEmision, Estado, Total)
                OUTPUT INSERTED.IdOrdenCompra
                VALUES (@IdProveedor, @IdUsuario, @FechaEmision, @Estado, @Total);";

            const string insertDetalle = @"
                INSERT INTO DetalleOrdenCompra (IdOrdenCompra, IdRefaccion, CantidadSolicitada, CostoUnitario, Subtotal)
                VALUES (@IdOrdenCompra, @IdRefaccion, @CantidadSolicitada, @CostoUnitario, @Subtotal);";

            using (var conexion = ConexionDB.Instancia.CrearConexion())
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        int idOrdenGenerada;
                        using (var cmdOrden = new SqlCommand(insertOrden, conexion, transaccion))
                        {
                            cmdOrden.Parameters.AddWithValue("@IdProveedor", orden.IdProveedor);
                            cmdOrden.Parameters.AddWithValue("@IdUsuario", orden.IdUsuario);
                            cmdOrden.Parameters.AddWithValue("@FechaEmision", orden.FechaEmision);
                            cmdOrden.Parameters.AddWithValue("@Estado", orden.Estado);
                            cmdOrden.Parameters.AddWithValue("@Total", orden.Total);

                            idOrdenGenerada = (int)cmdOrden.ExecuteScalar();
                        }

                        foreach (var detalle in orden.Detalles)
                        {
                            using (var cmdDetalle = new SqlCommand(insertDetalle, conexion, transaccion))
                            {
                                cmdDetalle.Parameters.AddWithValue("@IdOrdenCompra", idOrdenGenerada);
                                cmdDetalle.Parameters.AddWithValue("@IdRefaccion", detalle.IdRefaccion);
                                cmdDetalle.Parameters.AddWithValue("@CantidadSolicitada", detalle.CantidadSolicitada);
                                cmdDetalle.Parameters.AddWithValue("@CostoUnitario", detalle.CostoUnitario);
                                cmdDetalle.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                                cmdDetalle.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                        orden.IdOrdenCompra = idOrdenGenerada;
                        return idOrdenGenerada;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}