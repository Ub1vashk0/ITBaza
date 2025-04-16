using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class ResourceRole
{
    public int Id { get; set; }

    public int ResourceId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual Resource? Resource { get; set; } = null!;
}
