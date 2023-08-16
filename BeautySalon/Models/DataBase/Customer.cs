﻿using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Customer
{
    public int IdCustomer { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual UserAdmin? IdUserNavigation { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
