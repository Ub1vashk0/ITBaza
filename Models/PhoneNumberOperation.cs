using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class PhoneNumberOperation
{
    public int Id { get; set; }

    public int PhoneNumberId { get; set; }

    public int PersonId { get; set; }

    public DateTime ActionDate { get; set; }

    public string Action { get; set; } = null!;

    public int ExecutorId { get; set; }

    public virtual SystemUser? Executor { get; set; } = null!; 

    public virtual Person? Person { get; set; } = null!;

    public virtual PhoneNumber? PhoneNumber { get; set; } = null!;
}

