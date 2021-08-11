using System;
using MediatR;

namespace OrderService.Features.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid ItemId { get; set; }
        public int Count { get; set; }
    }
}