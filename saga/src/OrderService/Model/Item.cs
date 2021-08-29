using System;
using System.Collections.Generic;

namespace OrderService.Model
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Order> Orders { get; set; }
    }
}