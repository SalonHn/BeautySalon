﻿using System;
using System.Collections.Generic;

namespace BeautySalon.Models.DataBase;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Idi { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public int Age { get; set; }

    public bool? EmployeeActive { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateModify { get; set; }

    public int IdRole { get; set; }

    public virtual RoleEmployee IdRoleNavigation { get; set; } = null!;
}
