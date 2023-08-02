using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class TypeUser
{
    public int IdType { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<UserAdmin> UserAdmins { get; set; } = new List<UserAdmin>();
}
