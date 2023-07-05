using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class InvoiceDetail
{
    public int IdInvoice { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int IdTypeTax { get; set; }

    public double Tax { get; set; }

    public virtual Tax IdTypeTaxNavigation { get; set; } = null!;
}
