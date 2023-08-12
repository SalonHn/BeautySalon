using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class ServiceDetail
{
    public int IdService { get; set; }

    public int IdProduct { get; set; }

    public double Quantity { get; set; }

    public int Id { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Product IdServiceNavigation { get; set; } = null!;
}
