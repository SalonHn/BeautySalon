using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class HoursAvailable
{
    public int IdHour { get; set; }

    public string Hour { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Timetable> TimetableCloseHourNavigations { get; set; } = new List<Timetable>();

    public virtual ICollection<Timetable> TimetableOpenHourNavigations { get; set; } = new List<Timetable>();
}
