using System;

namespace SIGERE.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Existencia { get; set; }
        public int StockMinimo { get; set; }
        public string Ubicacion { get; set; }
        public int? IdProveedor { get; set; }
        public string Estatus { get; set; }

        public bool EsInvarianteValido()
        {
            return Precio >= 0 && Existencia >= 0 && StockMinimo >= 0 &&
                   (Estatus == "Activo" || Estatus == "Descontinuado");
        }
    }
}