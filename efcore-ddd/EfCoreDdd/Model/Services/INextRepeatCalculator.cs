using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Services;

public interface INextRepeatCalculator
{
    DateTime Calculate(Drawer drawer);
}

internal class SimpleNextRepeatCalculator : INextRepeatCalculator
{
    public DateTime Calculate(Drawer drawer)
    {
        return DateTime.Now.AddDays(drawer.Correct);
    }
}