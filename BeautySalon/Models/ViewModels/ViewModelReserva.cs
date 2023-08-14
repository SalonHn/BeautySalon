namespace BeautySalon.Models.ViewModels
{
    public class ViewModelReserva
    {
        public int Id { get; set; }
        public DateTime fecha { get; set; }
        public string? Name { get; set; }
        public string? hora { get; set; }
        public bool? Vip { get; set; }
    }
}
