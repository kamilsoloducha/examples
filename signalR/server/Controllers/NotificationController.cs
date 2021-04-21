using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<MessageHub, IMessageHubClient> hubContext;
        private readonly IConnectionStore connectionStore;

        public MessageController(IHubContext<MessageHub, IMessageHubClient> hubContext,
         IConnectionStore connectionStore)
        {
            this.hubContext = hubContext;
            this.connectionStore = connectionStore;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(SendMessageRequest request, CancellationToken cancellationToken)
        {
            var connectionId = this.connectionStore.GetConnection(Guid.Parse(request.UserId));
            await hubContext.Clients.AllExcept(connectionId).SendMessage(new Message
            {
                Content = request.Message,
                UserName = request.UserName
            });
            return Ok();
        }
    }

    public class SendMessageRequest
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
