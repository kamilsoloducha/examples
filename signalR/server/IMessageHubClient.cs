using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Server
{
    public interface IMessageHubClient
    {
        Task BroadcastMessage(string message);
    }

    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task SendMessage(string user, string message)
        {

        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
