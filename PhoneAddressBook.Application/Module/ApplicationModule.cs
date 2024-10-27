using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Application.Services;

namespace PhoneAddressBook.Application.Module;

public static class ApplicationModule
{
    public static void AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
    }
}