using EfCoreDdd.Model.Services;
using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Details : Entity, IAggregateRoot
{
    public SideType SideType { get; }
    public Drawer Drawer { get; private set; }
    public Counter Counter { get; private set; }
    
    public bool IsQuestion { get; }
    public DateTime NextRepeat { get; private set; }

    public virtual Card Card { get; }

    private Details()
    {
    }

    public Details(SideType sideType, bool isQuestion, Card card) : this()
    {
        SideType = sideType;
        IsQuestion = isQuestion;
        Drawer = new Drawer();
        Counter = new Counter();
        NextRepeat = new DateTime();
        Card = card;
    }

    public void AnswerCorrect(INextRepeatCalculator nextRepeatCalculator)
    {
        var increase = GetIncrease();
        Drawer = Drawer.Increase(increase);
        Counter = Counter.Increase();
        NextRepeat = nextRepeatCalculator.Calculate(Drawer);
    }

    public void AnswerWrong(DateTime now)
    {
        Counter = Counter.Increase();
        Drawer = new Drawer();
        NextRepeat = now.AddDays(1);
    }

    public void AnswerAccepted(DateTime now)
    {
        Counter = Counter.Increase();
        NextRepeat = now.AddDays(1);
    }

    private int GetIncrease() => Drawer.Correct > Counter.Value ? 3 : 1;

}