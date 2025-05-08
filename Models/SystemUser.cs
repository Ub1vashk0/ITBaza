using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class SystemUser
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string? FullName { get; set; }

    public int RoleId { get; set; }

    public string? InfoAccessRights { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LastPasswordChangeDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AccessOperation> AccessOperations { get; set; } = new List<AccessOperation>();

    public virtual Role? Role { get; set; } = null!;
    public virtual ICollection<PhoneNumberOperation> PhoneNumberOperations { get; set; } = new List<PhoneNumberOperation>();
}
