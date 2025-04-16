using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class AccessOperation
{
    public int Id { get; set; }

    public int AccessId { get; set; }

    public int PersonId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    public int ExecutorId { get; set; }

    public virtual Access? Access { get; set; } = null!;

    public virtual SystemUser? Executor { get; set; } = null!;

    public virtual Person? Person { get; set; } = null!;
}
