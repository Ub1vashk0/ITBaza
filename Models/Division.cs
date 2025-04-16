using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Division
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; } = null!;

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
}
