namespace BeautySalon.Models.ViewModels
{
    public class ViewModelProduct
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? sku { get; set; }
        public string? categoria { get; set; }
        public decimal? price { get; set; }
        public double? stock { get; set; }
    }
}
