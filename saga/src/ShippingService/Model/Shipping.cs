using System;

namespace ShippingService.Model
{
    public class Shipping
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Order Order { get; set; }
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Shipping Shipping { get; set; }

    }
}