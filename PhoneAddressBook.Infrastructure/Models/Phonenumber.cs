namespace PhoneAddressBook.Infrastructure.Models;

public partial class Phonenumber
{
    public int Id { get; set; }

    public int Addressid { get; set; }

    public string Phonenumber1 { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;
}
