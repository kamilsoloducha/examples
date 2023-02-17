namespace EfCoreDdd.Model.ValueObjects;

public class Drawer
{
    public int Correct { get; }

    public Drawer()
    {
    }

    public Drawer(int correct)
    {
        if (correct < 0) throw new Exception();
        Correct = correct;
    }

    public Drawer Increase(int increase = 1) => new(Correct + increase);
}