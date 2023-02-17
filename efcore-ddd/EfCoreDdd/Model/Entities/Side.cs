using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Side : Entity
{
    public SideType Type { get; }
    public Label Label { get; }
    public Example Example { get; }

    public virtual Card Card { get; }

    private Side()
    {
    }

    public Side(SideType type, Label label, Example example, Card card) : this()
    {
        if (type == SideType.Undefined) throw new Exception("");
        
        Type = type;
        Label = label;
        Example = example;
        Card = card;
    }
}