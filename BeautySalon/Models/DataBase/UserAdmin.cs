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

    public string UserEmail { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
