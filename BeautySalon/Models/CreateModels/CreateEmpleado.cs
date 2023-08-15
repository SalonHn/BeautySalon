namespace BeautySalon.Models.CreateModels
{
    public class CreateEmpleado
    {
        //Datos para tabla empleado
        public int IdEmpleado { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public int IdRole { get; set; }
        public string Dni { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }


        //Datos para la tabla usuario
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string UserPasswordConfirm { get; set; } = null!;
        public int IdType { get; set; }
    }
}
