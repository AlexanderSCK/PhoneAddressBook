using System;
using System.Collections.Generic;

namespace PhoneAddressBook.Infrastructure.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public Guid Personid { get; set; }

    public int Type { get; set; }

    public string Address1 { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Phonenumber> Phonenumbers { get; set; } = new List<Phonenumber>();
}
