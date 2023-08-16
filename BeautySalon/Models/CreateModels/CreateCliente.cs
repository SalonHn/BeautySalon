namespace BeautySalon.Models.CreateModels
{
    public class CreateCliente
    {
        //Datos del cliente
        public int IdCustomer { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;


        //Datos de Ususario
        public int IdUser { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string UserPasswordConfirm { get; set; } = null!;
    }
}
