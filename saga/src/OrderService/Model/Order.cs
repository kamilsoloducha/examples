using System;

namespace OrderService.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }
        public OrderStatus Status { get; set; }

        public Order()
        {
            Status = OrderStatus.Added;
        }
    }
}