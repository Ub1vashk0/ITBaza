using ITBaza.Models.Enums;
using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class Person
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateOnly? DismissalDate { get; set; }

    public string PersonalPhone { get; set; } = null!;

    public string? Email { get; set; }

    public int? VacationId { get; set; }

    public WorkType? WorkType { get; set; }

    public int? PlacementId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AccessOperation> AccessOperations { get; set; } = new List<AccessOperation>();

    public virtual ICollection<PhoneNumberOperation> PhoneNumberOperations { get; set; } = new List<PhoneNumberOperation>();

    public virtual Placement? Placement { get; set; }

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();

    public virtual Vacation? Vacation { get; set; }
}
