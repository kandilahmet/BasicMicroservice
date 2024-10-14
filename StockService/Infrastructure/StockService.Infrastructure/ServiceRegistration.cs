using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MassTransit;
using CrossCutting.Contracts;
using StockService.Infrastructure.MessageBroker.Consumers;

namespace StockService.Infrastructure.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(
               configurator =>
               {
                   #region AddConsumer
                   configurator.AddConsumer<OrderCreatedEventConsumer>();

                   #endregion

                   configurator.UsingRabbitMq((_context, _configurator) =>
                   {
                       _configurator.Host(configuration.GetSection("RabbitMQ").Value);

                       //Hangi Consumer'ın hangi kuyruğu dinliyeceğini belirtiyoruz ve Consumer'ın konfigürasyonlarını yapılandırıyoruz. 
                       _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(_context));


                   });
               });
        }
    }
}
