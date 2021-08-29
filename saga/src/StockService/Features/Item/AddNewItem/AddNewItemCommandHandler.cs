using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StockService.Database;

namespace StockService.Feature.Item.AddNewItem
{

    public class AddNewItemCommandHandler : IRequestHandler<AddNewItemCommand, Guid>
    {
        private readonly StockDbContext dbContext;

        public AddNewItemCommandHandler(StockDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Handle(AddNewItemCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            var newItem = new Model.Item
            {
                Id = id,
                Name = request.Name,
                Count = request.Count
            };

            await dbContext.Items.AddAsync(newItem, cancellationToken);

            return id;
        }
    }

}