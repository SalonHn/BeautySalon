using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class RoleEmployee
{
    public int IdRole { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
