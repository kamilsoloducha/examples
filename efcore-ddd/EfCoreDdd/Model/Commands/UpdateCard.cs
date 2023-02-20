using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Commands;

public record UpdateCard(Side Front, Side Back, bool IsTicked);

public record Side(Label Label, Example Example, string Comment, bool UseAsQuestion);
