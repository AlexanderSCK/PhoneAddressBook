using System.Net;

namespace PhoneAddressBook.API.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }
}