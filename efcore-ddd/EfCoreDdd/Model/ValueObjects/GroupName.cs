namespace EfCoreDdd.Model.ValueObjects;

public class GroupName
{
    public string Value { get; }
    
    private GroupName(){}

    public GroupName(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new Exception();
        Value = text.Trim();
    }
}