namespace BeautySalon.Models.CreateModels
{
    public class DetalleFactura
    {
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int IdTypeTax { get; set; }
        public double Tax { get; set; }
    }
}
