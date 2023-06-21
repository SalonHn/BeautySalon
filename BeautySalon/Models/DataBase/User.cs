using System;
using System.Collections.Generic;

namespace BeautySalon.Models.Database;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public DateTime DateCreate { get; set; }
}
