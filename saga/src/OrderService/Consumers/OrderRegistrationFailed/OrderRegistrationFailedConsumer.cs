using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using OrderService.Model;

namespace OrderService.Consumers.OrderRegistrationFailed
{
    public class OrderRegistrationFailedConsumer : IConsumer<Events.OrderRegistrationFailed>
    {
        private readonly OrderDbContext dbContext;

        public OrderRegistrationFailedConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Events.OrderRegistrationFailed> context)
        {
            var order = await dbContext.Orders.SingleOrDefaultAsync(x => x.Id == context.Message.OrderId, context.CancellationToken);

            if (order == null)
                throw new Exception($"Order with Id {context.Message.OrderId} has not been found.");

            order.Status = OrderStatus.Cancelled;

            dbContext.Update(order);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}