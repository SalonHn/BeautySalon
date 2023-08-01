using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Permission
{
    public int IdUser { get; set; }

    public int IdModule { get; set; }

    public bool StatusPermission { get; set; }

    public virtual Module IdModuleNavigation { get; set; } = null!;

    public virtual UserAdmin IdUserNavigation { get; set; } = null!;
}
