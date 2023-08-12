using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class AppointmentDetail
{
    public int IdAppointment { get; set; }

    public int IdService { get; set; }

    public int HourService { get; set; }

    public int Id { get; set; }

    public virtual HoursAvailable HourServiceNavigation { get; set; } = null!;

    public virtual Appointment IdAppointmentNavigation { get; set; } = null!;

    public virtual Product IdServiceNavigation { get; set; } = null!;
}
