using System;
using System.Threading;
using System.Threading.Tasks;
using Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using OrderService.Model;

namespace OrderService.Features.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly OrderDbContext dbContext;
        private readonly IPublishEndpoint publishEndpoint;

        public CreateOrderCommandHandler(OrderDbContext dbContext,
        IPublishEndpoint publishEndpoint)
        {
            this.dbContext = dbContext;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var item = await dbContext.Items.SingleOrDefaultAsync(x => x.Id == request.ItemId, cancellationToken);
            if (item == null)
                throw new Exception($"Item with Id {request.ItemId} has not been found.");

            if (request.Count <= 0)
                throw new Exception($"Parameter {nameof(request.Count)} cannot be less or equal zero");

            var orderId = Guid.NewGuid();

            var newOrder = new Order
            {
                Id = orderId,
                Item = item,
                Count = request.Count
            };

            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.AddAsync(newOrder, cancellationToken);
            await publishEndpoint.Publish(new OrderCreated
            {
                OrderId = orderId,
                ItemId = item.Id,
                Count = request.Count
            }, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return orderId;
        }
    }
}