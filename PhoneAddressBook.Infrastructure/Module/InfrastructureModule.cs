using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Infrastructure.Mappings;
using PhoneAddressBook.Infrastructure.Models;
using PhoneAddressBook.Infrastructure.Repository;

namespace PhoneAddressBook.Infrastructure.Module
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PhoneAddressBookDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
