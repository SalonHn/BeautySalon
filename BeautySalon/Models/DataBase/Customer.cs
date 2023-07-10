using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Models.DataBase;

public partial class Customer : ValidationAttribute
{
    public int IdCustomer { get; set; }

    [Required(ErrorMessage="Nombre Requerido")]
    [MaxLength(50, ErrorMessage = "Maximo de caracteres superado")]
    [MinLength(3, ErrorMessage = "Nombre invalido")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Telefono Requerido")]
    [MaxLength(20, ErrorMessage = "Maximo de digitos superado")]
    [MinLength(8, ErrorMessage = "Telefono invalido")]
    public string Phone { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    [Required(ErrorMessage = "Pin Requerido")]
    [MaxLength(6, ErrorMessage = "El numero maximo de digitos es 6")]
    [MinLength(4, ErrorMessage = "El numero minimo de digitos es 4")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Solo se aceptan numeros")]
    public string PinCustomer { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
