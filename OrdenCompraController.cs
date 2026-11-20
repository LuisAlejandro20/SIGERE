using System;
using System.Collections.Generic;
using System.Linq;
using SIGERE.DAO;
using SIGERE.Models;

namespace SIGERE.Controllers
{
    public class OrdenCompraController
    {
        private readonly OrdenCompraDAO _ordenCompraDao;

        public OrdenCompraController()
        {
            _ordenCompraDao = OrdenCompraDAO.Instancia;
        }

        public List<RefaccionStockCritico> ConsultarAlertasStockMinimo()
        {
            return _ordenCompraDao.ObtenerRefaccionesEnStockMinimo();
        }

        public List<OrdenCompra> GenerarOrdenesPorStockMinimo(int idUsuarioSolicitante)
        {
            if (idUsuarioSolicitante <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuarioSolicitante), "Usuario solicitante inválido.");

            var refaccionesCriticas = _ordenCompraDao.ObtenerRefaccionesEnStockMinimo();
            if (refaccionesCriticas.Count == 0)
                throw new InvalidOperationException("No existen refacciones en nivel de stock mínimo.");

            var gruposPorProveedor = refaccionesCriticas.GroupBy(r => r.IdProveedor);
            var ordenesGeneradas = new List<OrdenCompra>();

            foreach (var grupo in gruposPorProveedor)
            {
                var orden = ConstruirOrdenDesdeCriticos(grupo.Key, idUsuarioSolicitante, grupo.ToList());
                _ordenCompraDao.RegistrarOrdenConDetalles(orden);
                ordenesGeneradas.Add(orden);
            }

            return ordenesGeneradas;
        }

        public OrdenCompra GenerarOrdenSeleccionada(int idProveedor, int idUsuarioSolicitante, List<RefaccionStockCritico> seleccionadas)
        {
            if (idProveedor <= 0)
                throw new ArgumentOutOfRangeException(nameof(idProveedor));
            if (idUsuarioSolicitante <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuarioSolicitante));
            if (seleccionadas == null || seleccionadas.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una refacción para la orden.", nameof(seleccionadas));
            if (seleccionadas.Any(r => r.IdProveedor != idProveedor))
                throw new InvalidOperationException("Todas las refacciones de la orden deben pertenecer al mismo proveedor.");

            var orden = ConstruirOrdenDesdeCriticos(idProveedor, idUsuarioSolicitante, seleccionadas);
            _ordenCompraDao.RegistrarOrdenConDetalles(orden);
            return orden;
        }

        private static OrdenCompra ConstruirOrdenDesdeCriticos(
            int idProveedor,
            int idUsuario,
            List<RefaccionStockCritico> refacciones)
        {
            var orden = new OrdenCompra
            {
                IdProveedor = idProveedor,
                IdUsuario = idUsuario,
                FechaEmision = DateTime.Now,
                Estado = "Pendiente"
            };

            foreach (var item in refacciones)
            {
                int cantidad = item.CantidadSugerida;
                if (cantidad <= 0)
                    throw new InvalidOperationException($"Cantidad calculada inválida para la refacción {item.Codigo}.");

                decimal subtotal = cantidad * item.CostoCompra;
                orden.Detalles.Add(new DetalleOrdenCompra
                {
                    IdRefaccion = item.IdRefaccion,
                    CantidadSolicitada = cantidad,
                    CostoUnitario = item.CostoCompra,
                    Subtotal = subtotal
                });
            }

            orden.Total = orden.Detalles.Sum(d => d.Subtotal);
            return orden;
        }
    }
}