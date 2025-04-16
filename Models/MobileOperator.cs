using System;
using System.Collections.Generic;

namespace ITBaza.Models;

public partial class MobileOperator
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Corporation { get; set; }

    public int IdCountry { get; set; }

    public string Code { get; set; } = null!;

    public virtual Country? IdCountryNavigation { get; set; } = null!;

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
