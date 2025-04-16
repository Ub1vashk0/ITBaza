using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class ResourceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
