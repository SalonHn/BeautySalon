using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Holiday
{
    public int IdHoliday { get; set; }

    public DateTime Date { get; set; }

    public string Reason { get; set; } = null!;
}
