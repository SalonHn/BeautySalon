using BeautySalon.Models.ViewModels;

namespace BeautySalon.Models.CreateModels
{
    public class CreateFactura
    {
        public int IdCustomer { get; set; }
        public string NameCustomer { get; set; } = null!;
        public int IdTipoPago { get; set; }
        public decimal? Recibido { get; set; }

        public List<BuscarProducto> DetalleFactura { get; set; } = null!;
    }
}
