using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Access
{
    public int Id { get; set; }

    public int ResourceId { get; set; }

    public string Login { get; set; } = null!;

    public int ResourceRoleId { get; set; }

    public virtual ICollection<AccessOperation> AccessOperations { get; set; } = new List<AccessOperation>();

    public virtual Resource? Resource { get; set; } = null!;

    public virtual ResourceRole? ResourceRole { get; set; } = null!;
}
