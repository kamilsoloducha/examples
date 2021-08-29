using System;

namespace Events
{
    public class OrderShippingFailed
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public int Count { get; set; }
    }
}