namespace BeautySalon.Models.ViewModels
{
    public class ViewModelAllUser
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? UserName { get; set; }
        public string? genero { get; set; }
        public DateTime CreateDate { get; set; }
        public bool userActive { get; set; }
    }
}
