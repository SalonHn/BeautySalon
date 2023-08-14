using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class EstadoReserva
{
    public int IdEstado { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
