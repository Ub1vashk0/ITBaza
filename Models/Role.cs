using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PhoneNumberOperation> PhoneNumberOperations { get; set; } = new List<PhoneNumberOperation>();

    public virtual ICollection<SystemUser> SystemUsers { get; set; } = new List<SystemUser>();
}
