using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class UserAdmin
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public bool UserActive { get; set; }

    public DateTime UserDateCreate { get; set; }

    public DateTime UserDateModify { get; set; }

    public int? IdType { get; set; }

    public string? TokenPush { get; set; }

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual TypeUser? IdTypeNavigation { get; set; }

    public virtual ICollection<Membresium> Membresia { get; set; } = new List<Membresium>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
