using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int IdCustomer { get; set; }

    public int IdServicio { get; set; }

    public DateTime Fecha { get; set; }

    public int IdHora { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual HoursAvailable IdHoraNavigation { get; set; } = null!;

    public virtual Product IdServicioNavigation { get; set; } = null!;
}
