namespace BeautySalon.Models.ViewModels
{
    public class BuscarProducto
    {
        public int idProducto { get; set; }
        public string name { get; set; } = null!;
        public decimal precio { get; set; }
        public decimal tax { get; set; }
        public int cantidad { get; set; }
        public int typeTax { get; set; }
    }
}
