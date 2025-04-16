using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MobileOperator> MobileOperators { get; set; } = new List<MobileOperator>();

    public virtual ICollection<Placement> Placements { get; set; } = new List<Placement>();
}
