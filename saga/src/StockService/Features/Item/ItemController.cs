using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockService.Feature.Item.AddNewItem;

namespace StockService.Feature.Item
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IMediator mediater;

        public ItemController(IMediator mediater)
        {
            this.mediater = mediater;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewItem(AddNewItemCommand command)
            => new JsonResult(await mediater.Send(command));
    }
}