using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using StockService.Infrastructure.Persistence.MongoDB;
using CrossCutting.Abstractions.Repositories.MongoDB; 
using MongoDB.Driver;


namespace StockService.Infrastructure.Persistence
{
    public static class ServiceRegistration  
    {
        public static void AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(options => configuration.GetSection(nameof(MongoDBSettings)).Bind(options));

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddTransient(typeof(IMongoDBReadRepository<>), typeof(MongoDBReadRepository<>));
            services.AddTransient(typeof(IMongoDBWriteRepository<>), typeof(MongoDBWriteRepository<>));

        }


    }
}
