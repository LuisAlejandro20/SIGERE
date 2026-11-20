using System;
using System.Collections.Generic;

namespace SIGERE.Models
{
    public class OrdenCompra
    {
        public int IdOrdenCompra { get; set; }
        public int IdProveedor { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public decimal Total { get; set; }
        public List<DetalleOrdenCompra> Detalles { get; set; } = new List<DetalleOrdenCompra>();
    }

    public class DetalleOrdenCompra
    {
        public int IdDetalle { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdRefaccion { get; set; }
        public int CantidadSolicitada { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}