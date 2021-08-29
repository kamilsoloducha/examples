using System;

namespace Events
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }

        public Guid ItemId { get; set; }

        public int Count { get; set; }
    }

    public class OrderRegisteredInStock
    {
        public Guid OrderId { get; set; }

        public Guid ItemId { get; set; }

        public int Count { get; set; }
    }
}