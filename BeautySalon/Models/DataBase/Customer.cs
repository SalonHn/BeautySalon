using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Customer
{
    public int IdCustomer { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int PinCustomer { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
