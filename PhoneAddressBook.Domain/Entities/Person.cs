namespace PhoneAddressBook.Domain.Entities;

public class Person
{
    public Person()
    {
        Addresses = [];
    }
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public ICollection<Address> Addresses { get; set; }
}