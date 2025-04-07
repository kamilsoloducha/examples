namespace Server.Endpoints;

public record SendMessageRequest(string Message, string UserName, string UserId);