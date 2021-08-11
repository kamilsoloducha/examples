using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using OrderService.Model;

namespace OrderService.Consumers.OrderShippingFailed
{
    public class OrderShippingFailedConsumer : IConsumer<Events.OrderShippingFailed>
    {
        private readonly OrderDbContext dbContext;

        public OrderShippingFailedConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Events.OrderShippingFailed> context)
        {
            var order = await dbContext.Orders.SingleOrDefaultAsync(x => x.Id == context.Message.OrderId, context.CancellationToken);

            order.Status = OrderStatus.Cancelled;

            dbContext.Update(order);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}