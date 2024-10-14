using CrossCutting.Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.MessageBroker.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint _publisher) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (false)//ödeme başarılı ise
            {
                PaymentCompletedEvent paymentCompletedEvent = new() { OrderId = context.Message.OrderId };
               await _publisher.Publish(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new() { OrderId = context.Message.OrderId, Message = "Öedme İşlemi Başarısız " };
               await _publisher.Publish(paymentFailedEvent);

            }
           
        }
    }
}
