using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
}
