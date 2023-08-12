using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Appointment
{
    public int IdAppointment { get; set; }

    public int IdCustomer { get; set; }

    public DateTime DateAppointment { get; set; }

    public string StatusAppointment { get; set; } = null!;

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();

    public virtual Customer IdCustomerNavigation { get; set; } = null!;
}
