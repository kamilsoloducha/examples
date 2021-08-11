using System;
using System.Threading.Tasks;
using MassTransit;
using ShippingService.Database;
using ShippingService.Model;

namespace ShippingService.Consumers.OrderRegisteredInStock
{
    public class OrderRegisteredInStockConsumer : IConsumer<Events.OrderRegisteredInStock>
    {
        private readonly ShippingDbContext dbContext;

        public OrderRegisteredInStockConsumer(ShippingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Events.OrderRegisteredInStock> context)
        {
            if (context.Message.Count > 100)
            {
                await context.Publish(new Events.OrderShippingFailed
                {
                    OrderId = context.Message.OrderId,
                    Count = context.Message.Count,
                    ItemId = context.Message.ItemId
                }, context.CancellationToken);
                return;
            }

            var newOrder = new Order
            {
                Id = context.Message.OrderId,
            };

            var newShipping = new Shipping
            {
                Id = Guid.NewGuid(),
                Order = newOrder
            };

            using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);

            await dbContext.Shippings.AddAsync(newShipping, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(new Events.OrderShipped { OrderId = context.Message.OrderId }, context.CancellationToken);

            await transaction.CommitAsync(context.CancellationToken);
        }
    }
}