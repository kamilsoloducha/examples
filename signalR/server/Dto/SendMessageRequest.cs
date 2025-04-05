namespace Server.Dto;

public record SendMessageRequest(string Message, string UserName, string UserId);