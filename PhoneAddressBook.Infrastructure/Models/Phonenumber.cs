using System;
using System.Collections.Generic;

namespace PhoneAddressBook.Infrastructure.Models;

public partial class Phonenumber
{
    public Guid Id { get; set; }

    public Guid Addressid { get; set; }

    public string Phonenumber1 { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;
}
