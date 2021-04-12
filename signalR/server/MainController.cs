using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MessageHub, IMessageHubClient> hubContext;

        public MainController(IHubContext<MessageHub, IMessageHubClient> hubContext)
        {
            this.hubContext = hubContext;
        }


        [HttpGet("{value}")]
        public IActionResult Send([FromRoute] string value)
        {
            hubContext.Clients.All.BroadcastMessage(value);
            return Ok();
        }
    }
}
