using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models.DataBase;

public partial class Product
{
    public int IdProduct { get; set; }

    public string NameProduct { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int IdCategory { get; set; }

    public decimal Price { get; set; }

    public double Stock { get; set; }

    public int StockMinimum { get; set; }

    public string Sku { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime ModifyDate { get; set; }

    public bool Featured { get; set; }

    public int IdTax { get; set; }

    public int IdSkill { get; set; }

    public string? ImageProduct { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; } 

    public virtual RoleEmployee? IdSkillNavigation { get; set; } 

    public virtual Tax? IdTaxNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<ServiceDetail> ServiceDetailIdProductNavigations { get; set; } = new List<ServiceDetail>();

    public virtual ICollection<ServiceDetail> ServiceDetailIdServiceNavigations { get; set; } = new List<ServiceDetail>();



    [NotMapped]
    public IFormFile? ImgFile { get; set; }
}
