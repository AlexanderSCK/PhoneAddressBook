using Microsoft.Extensions.DependencyInjection;
using PhoneAddressBook.API.Middleware;

namespace PhoneAddressBook.API.Module;

public static class ApiModule
{
    public static void AddApiModule(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}