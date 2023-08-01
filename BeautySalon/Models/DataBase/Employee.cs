using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models.DataBase;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public int Age { get; set; }

    public bool? EmployeeActive { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateModify { get; set; }

    public int IdRole { get; set; }

    public string Dni { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual RoleEmployee? IdRoleNavigation { get; set; }

    public virtual UserAdmin? IdUserNavigation { get; set; }

    //No Mapeado
    [NotMapped]
    public string UserName { get; set; } = null!;
    [NotMapped]
    public string UserPassword { get; set; } = null!;
    [NotMapped]
    public string UserPasswordConfirm { get; set; } = null!;
    [NotMapped]
    public string UserEmail { get; set; } = null!;
}
