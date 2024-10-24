using System;
using System.Collections.Generic;

namespace PhoneAddressBook.Infrastructure.Models;

public partial class Person
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
