using CrossCutting.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Infrastructure.MessageBroker.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(
             configurator =>
             {
                 #region AddConsumer
                 configurator.AddConsumer<StockReservedEventConsumer>();

                 #endregion

                 configurator.UsingRabbitMq((_context, _configurator) =>
                 {
                     _configurator.Host(configuration.GetSection("RabbitMQ").Value);

                     _configurator.ReceiveEndpoint(RabbitMQSettings.Payment_StockReservedEventQueue, e => e.ConfigureConsumer<StockReservedEventConsumer>(_context));

                 });
             });
        }
    }
}
