using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StockService.Database;

namespace StockService.Consumers.OrderCreated
{
    public class OrderCreatedConsumer : IConsumer<Events.OrderCreated>
    {
        private readonly StockDbContext dbContext;

        public OrderCreatedConsumer(StockDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Events.OrderCreated> context)
        {
            var item = await dbContext.Items.SingleOrDefaultAsync(x => x.Id == context.Message.ItemId, context.CancellationToken);

            if (item == null || item.Count < context.Message.Count)
                await context.Publish(new Events.OrderRegistrationFailed
                {
                    OrderId = context.Message.OrderId
                }, context.CancellationToken);

            item.Count -= context.Message.Count;

            using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);

            dbContext.Update(item);
            await dbContext.SaveChangesAsync(context.CancellationToken);

            await context.Publish(new Events.OrderRegisteredInStock
            {
                OrderId = context.Message.OrderId,
                ItemId = context.Message.ItemId,
                Count = context.Message.Count
            }, context.CancellationToken);

            await transaction.CommitAsync(context.CancellationToken);
        }
    }
}