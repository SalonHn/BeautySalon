using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdTipoPago { get; set; }

    public string? NumeroDeTarjeta { get; set; }

    public decimal Monto { get; set; }

    public decimal? Recibido { get; set; }

    public decimal? Cambio { get; set; }

    public string? Vence { get; set; }

    public string? Ccv { get; set; }

    public virtual TipoPago IdTipoPagoNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Membresium> Membresia { get; set; } = new List<Membresium>();
}
