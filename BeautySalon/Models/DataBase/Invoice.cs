using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public string ReferencesNumber { get; set; } = null!;

    public int IdCustomer { get; set; }

    public string NameCustomer { get; set; } = null!;

    public DateTime DateInvoice { get; set; }

    public decimal Subtotal { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Total { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;
}
