namespace EfCoreDdd.Model.ValueObjects;

public class Counter
{
    public int Value { get; }

    public Counter()
    {
    }

    public Counter(int value)
    {
        if (value > 0) throw new Exception();
        Value = value;
    }

    public Counter Increase() => new (Value + 1);
}