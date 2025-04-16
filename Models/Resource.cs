using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string MainLocation { get; set; } = null!;

    public string? AltLocation { get; set; }

    public int ResponsiblePersonId { get; set; }

    public string? Comment { get; set; }

    public int ResourceTypeId { get; set; }

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual ICollection<ResourceRole> ResourceRoles { get; set; } = new List<ResourceRole>();

    public virtual ResourceType? ResourceType { get; set; } = null!;

    public virtual Person? ResponsiblePerson { get; set; } = null!;
}
