using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Tax
{
    public int IdTax { get; set; }

    public string TypeTax { get; set; } = null!;

    public string DescriptionTax { get; set; } = null!;

    public double Tax1 { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
