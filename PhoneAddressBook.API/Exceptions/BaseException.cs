using System.Net;

namespace PhoneAddressBook.API.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode;
        public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
