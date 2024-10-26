using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PhoneAddressBook.API.Exceptions
{
    public class BadRequestException : BaseException
    {
        public IDictionary<string, string[]> Errors { get; }
        public BadRequestException(ModelStateDictionary modelState)
            : base("Please provide valid input", System.Net.HttpStatusCode.BadRequest)
        {
            Errors = modelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
        }
    }
}
