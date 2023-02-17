namespace EfCoreDdd.Model.ValueObjects;

public class Label
{
    public string Value { get; }

    public Label(string text)
    {
        text = text.Trim();

        if (string.IsNullOrEmpty(text)) throw new Exception("");
        Value = text;
    }
}