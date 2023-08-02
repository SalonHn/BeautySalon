using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Bitacora
{
    public int IdBitacora { get; set; }

    public DateTime Fecha { get; set; }

    public int Nivel { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? Usuario { get; set; }

    public string? DetallesAdicionales { get; set; }

    public virtual UserAdmin? UsuarioNavigation { get; set; }
}
