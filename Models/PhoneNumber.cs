using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class PhoneNumber
{
    public int Id { get; set; }

    public string? Number { get; set; }

    public int? OperatorId { get; set; }

    public string? Status { get; set; }

    public string? SimSerialNumber { get; set; }

    public string? Pin1 { get; set; }

    public string? Pin2 { get; set; }

    public string? Puk1 { get; set; }

    public string? Puk2 { get; set; }

    public string? Comment { get; set; }

    public virtual MobileOperator? Operator { get; set; }

    public virtual ICollection<PhoneNumberOperation> PhoneNumberOperations { get; set; } = new List<PhoneNumberOperation>();
}
