using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class TipoPago
{
    public int IdTipoPago { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
