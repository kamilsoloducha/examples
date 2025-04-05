using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Server;

public interface IMessageHubClient
{
    Task BroadcastMessage(string message);
    Task SendMessage(Message message);
    Task NewConnection(string connectionId);
    Task ConnectionLost(string connectionId);
}

public class Message
{
    public string Content { get; set; }
    public string UserName { get; set; }
}

public interface IConnectionStore
{
    IEnumerable<string> GetConnections();
    string GetConnection(Guid userId);
}


public class MessageHub : Hub<IMessageHubClient>, IConnectionStore
{

    public MessageHub(ILogger<MessageHub> logger,
        IHttpContextAccessor httpContext) : base()
    {
        this.logger = logger;
        this.httpContext = httpContext;
    }

    private readonly static IList<Connection> connections = new List<Connection>();
    private readonly ILogger<MessageHub> logger;
    private readonly IHttpContextAccessor httpContext;

    public string GetConnectionId() => Context.ConnectionId;

    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public async override Task OnConnectedAsync()
    {
        logger.LogInformation("New Message connection: {connectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
        connections.Add(Connection.Create(
            Context.ConnectionId,
            Context.User.Claims.FirstOrDefault(x => x.Type.Equals("id")).Value,
            Context.User.Identity.Name));
        await Clients.AllExcept(Context.ConnectionId).NewConnection(Context.ConnectionId);
    }

    public async override Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        var connectionToRemove = connections.SingleOrDefault(x => x.Id.Equals(Context.ConnectionId));
        if (connectionToRemove == null)
        {
            return;
        }
        connections.Remove(connectionToRemove);
        logger.LogInformation("Connection lost: {connectionId}", Context.ConnectionId);
        await Clients.AllExcept(Context.ConnectionId).ConnectionLost(Context.ConnectionId);
    }

    public IEnumerable<string> GetConnections() => connections.Select(x => x.Id);
    public string GetConnection(Guid userId) => connections.FirstOrDefault(x => x.UserId == userId).Id;

}

public class Connection
{
    public string Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public Connection() { }

    public static Connection Create(string id, string userId, string userName)
    {
        var userGuid = Guid.Parse(userId);
        return new Connection
        {
            Id = id,
            UserId = userGuid,
            UserName = userName
        };
    }
}