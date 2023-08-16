using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Membresium
{
    public int IdMembresia { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public bool? Estado { get; set; }

    public decimal Precio { get; set; }

    public int UserId { get; set; }

    public int? IdPago { get; set; }

    public virtual Pago? IdPagoNavigation { get; set; }

    public virtual UserAdmin User { get; set; } = null!;
}
