using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Timetable
{
    public int IdTimetable { get; set; }

    public string? Day { get; set; }

    public int OpenHour { get; set; }

    public int CloseHour { get; set; }

    public bool IsHoliday { get; set; }

    public virtual HoursAvailable CloseHourNavigation { get; set; } = null!;

    public virtual HoursAvailable OpenHourNavigation { get; set; } = null!;
}
