using System;
using MediatR;

namespace StockService.Feature.Item.AddNewItem
{
    public class AddNewItemCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

}