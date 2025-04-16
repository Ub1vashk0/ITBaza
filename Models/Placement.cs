using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Placement
{
    public int Id { get; set; }

    public int CountryId { get; set; }

    public string City { get; set; } = null!;

    public string? Office { get; set; }

    public virtual Country? Country { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
