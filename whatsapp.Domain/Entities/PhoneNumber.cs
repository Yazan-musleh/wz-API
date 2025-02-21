using System;
using System.Collections.Generic;

namespace whatsapp.Domain.Entities;

public partial class PhoneNumber
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber1 { get; set; } = null!;

    public string? Status { get; set; }
}
