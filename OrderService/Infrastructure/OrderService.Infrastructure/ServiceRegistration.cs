using CrossCutting.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Infrastructure.MessageBroker.Consumers; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        { 

            services.AddMassTransit(
                configurator =>
                {
                    #region AddConsumer
                    configurator.AddConsumer<StockNotReservedEventConsumer>();
                    configurator.AddConsumer<PaymentCompletedEventConsumer>();
                    configurator.AddConsumer<PaymentFailedEventConsumer>();
                    #endregion

                    configurator.UsingRabbitMq((_context, _configurator) =>
                    {
                        _configurator.Host(configuration.GetSection("RabbitMQ").Value);

                        //Hangi Consumer'ın hangi kuyruğu dinliyeceğini belirtiyoruz ve Consumer'ın konfigürasyonlarını yapılandırıyoruz. 
                        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotReservedEventQueue, e => e.ConfigureConsumer<StockNotReservedEventConsumer>(_context));
                        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(_context));
                        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(_context));

                    });
                });
        }

         
    }
}
