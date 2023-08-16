using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class InformacionDePago
{
    public int IdInfoPago { get; set; }

    public int IdUser { get; set; }

    public string NumeroTarjeta { get; set; } = null!;

    public string Ccv { get; set; } = null!;

    public string Vence { get; set; } = null!;

    public virtual UserAdmin IdUserNavigation { get; set; } = null!;
}
