namespace EfCoreDdd.Model.ValueObjects;

public class Example
{
    public string Value { get; }

    public Example(string text)
    {
        Value = text.Trim();
    }
}