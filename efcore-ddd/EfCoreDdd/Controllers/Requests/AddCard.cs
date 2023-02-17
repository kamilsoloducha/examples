namespace EfCoreDdd.Controllers.Requests;

public record AddCard(
    string Front,
    string Back,
    string FrontExample,
    string BackExample,
    long GroupId);