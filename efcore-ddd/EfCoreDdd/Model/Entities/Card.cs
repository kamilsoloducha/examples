using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Card : Entity, IAggregateRoot
{
    public virtual Side Front { get; }
    public virtual Side Back { get; }

    public virtual Details FrontDetails { get; }
    public virtual Details BackDetails { get; }

    public virtual Group Group { get; }

    protected Card()
    {
    }

    public Card(Label front, Label back, Example frontExample, Example backExample, Group group) : this()
    {
        Front = new Side(SideType.Front, front, frontExample, this);
        Back = new Side(SideType.Back, back, backExample, this);
        FrontDetails = new Details(SideType.Front, false, this);
        BackDetails = new Details(SideType.Back, false, this);
        Group = group;
    }
}