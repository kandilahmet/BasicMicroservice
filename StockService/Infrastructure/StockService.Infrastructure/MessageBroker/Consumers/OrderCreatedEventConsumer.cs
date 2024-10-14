using CrossCutting.Abstractions.Repositories.MongoDB;
using CrossCutting.Contracts;
using CrossCutting.Contracts.Events;
using MassTransit;
using MassTransit.Transports;
using StockService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.MessageBroker.Consumers
{
    public class OrderCreatedEventConsumer(IMongoDBWriteRepository<Stock> mongoDBWriteRepository
        , IMongoDBReadRepository<Stock> mongoDBReadRepository
        ,ISendEndpointProvider sendEndpointProvider
        ,IPublishEndpoint publisher) 
        : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stocks = await mongoDBReadRepository.GetWhere(x => context.Message.OrderItems.Exists(oi => oi.ProductId == x.ProductId.ToString() && x.Count >= oi.Count));

            //if (stocks.Count == 0) { return; }

            bool allExists = false;

            foreach (var oi in context.Message.OrderItems)
            {
                if (stocks.Exists(s => s.ProductId.ToString() == oi.ProductId))
                {
                    allExists = true;
                    stocks.FirstOrDefault(s => s.ProductId.ToString() == oi.ProductId).Count -= oi.Count;
                }
                else
                {
                    allExists = false; 
                }
            }

            if (!allExists)
            {
                var stockNotReservedEvent = new StockNotReservedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Ürün Stoklarda kalmamıştır"
                };

                //ISendEndpoint _sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Order_StockNotReservedEventQueue}"));
                await publisher.Publish(stockNotReservedEvent);
                return;

            }

            await mongoDBWriteRepository.UpdateRange(stocks);

            var stockReservedEvent = new StockReservedEvent()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                TotalPrice = context.Message.TotalPrice,
            };

            ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
            await sendEndpoint.Send(stockReservedEvent);

        }
    }
}
