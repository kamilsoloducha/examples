using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Server.Dto;

namespace Server.Endpoints;

public static class MessagesEndpoints
{
    public static WebApplication AddMessageSend(this WebApplication app)
    {
        app.MapPost("message/send", async (
            SendMessageRequest request,
            IHubContext<MessageHub, IMessageHubClient> hubContext,
            IConnectionStore connectionStore) =>
        {
            var connectionId = connectionStore.GetConnection(Guid.Parse(request.UserId));
            await hubContext.Clients.AllExcept(connectionId).SendMessage(new Message
            {
                Content = request.Message,
                UserName = request.UserName
            });
            return Results.Ok();
        });
        return app;
    }
}