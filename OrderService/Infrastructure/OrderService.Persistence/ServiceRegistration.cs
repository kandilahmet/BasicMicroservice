using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using Application.Abstractions.Repositories.EFCore; 
using OrderService.Infrastructure.Persistence.EFCore.Repositories;
using OrderService.Infrastructure.Persistence.EFCore.DbContexts;

namespace OrderService.Infrastructure.Persistence
{
    public static class ServiceRegistration  
    {
        public static void AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<MsSqlOrderDbContext>(options => options.UseSqlServer(configuration.GetSection("MsSqlConnectionString").Value));
            string asda = configuration.GetSection("MsSqlConnectionString").Value;
            services.AddTransient(typeof(IEFCoreReadRepository<>), typeof(EFCoreReadRepository<>));
            services.AddTransient(typeof(IEFCoreWriteRepository<>), typeof(EFCoreWriteRepository<>));

        }
         
    }
}
