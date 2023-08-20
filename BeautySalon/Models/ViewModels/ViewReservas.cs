namespace BeautySalon.Models.ViewModels
{
    public class ViewReservas
    {
        public int id { get; set; }
        public string nameServicio { get; set; } = null!;
        public string user { get; set; } = null!;
        public string hora { get; set; } = null!;
        public DateTime fecha { get; set; }
        public bool vip { get; set; }
    }
}
