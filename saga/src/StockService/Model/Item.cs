using System;

namespace StockService.Model
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }
}