using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StockService.Database;

namespace StockService.Consumers.OrderShippingFailed
{
    public class OrderShippingFailedConsumer : IConsumer<Events.OrderShippingFailed>
    {
        private readonly StockDbContext dbContext;

        public OrderShippingFailedConsumer(StockDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Events.OrderShippingFailed> context)
        {
            var item = await dbContext.Items.SingleOrDefaultAsync(x => x.Id == context.Message.ItemId);
            item.Count += context.Message.Count;

            dbContext.Update(item);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}