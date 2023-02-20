namespace EfCoreDdd.Controllers.Requests;

public record UpdateCard(long CardId, Side Front, Side Back, bool IsTicked);

public record Side(string Label, string Example, string Comment, bool UseAsQuestion);