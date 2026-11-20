namespace SIGERE.Models
{
    public class RefaccionStockCritico
    {
        public int IdRefaccion { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdProveedor { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public decimal CostoCompra { get; set; }

        public int CantidadSugerida =>
            StockMaximo > StockActual ? StockMaximo - StockActual : (StockMinimo - StockActual) + 1;

        public decimal SubtotalEstimado => CantidadSugerida * CostoCompra;
    }
}