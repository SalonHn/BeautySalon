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

    public int? IdEstado { get; set; }

    public virtual UserAdmin IdCustomerNavigation { get; set; } = null!;

    public virtual EstadoReserva? IdEstadoNavigation { get; set; }

    public virtual HoursAvailable IdHoraNavigation { get; set; } = null!;

    public virtual Product IdServicioNavigation { get; set; } = null!;
}
