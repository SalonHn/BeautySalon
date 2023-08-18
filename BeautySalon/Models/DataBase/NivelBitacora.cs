using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class NivelBitacora
{
    public int Id { get; set; }

    public string Nivel { get; set; } = null!;

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();
}
