using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Vacation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int DivisionId { get; set; }

    public virtual Department? Department { get; set; } = null!;

    public virtual Division? Division { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
