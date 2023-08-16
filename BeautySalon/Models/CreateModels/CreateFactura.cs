namespace BeautySalon.Models.CreateModels
{
    public class CreateFactura
    {
        public int IdCustomer { get; set; }
        public string NameCustomer { get; set; } = null!;
        public int IdTipoPago { get; set; }
        public decimal? Recibido { get; set; }

        List<DetalleFactura> DetalleFactura { get; set; } = null!;
    }
}
