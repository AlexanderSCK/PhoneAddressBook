using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Application.Module
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IPersonService, PersonService>();

        }
    }
}
