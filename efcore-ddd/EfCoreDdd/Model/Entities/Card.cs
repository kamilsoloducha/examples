using EfCoreDdd.Model.Commands;
using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Card : Entity, IAggregateRoot
{
    public virtual Side Front { get; private set; }
    public virtual Side Back { get; private set; }

    public virtual Details FrontDetails { get; private set; }
    public virtual Details BackDetails { get; private set; }

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

    public void Update(UpdateCard command)
    {
        if (ShouldBeUpdated(Front, command.Front))
        {
            Front = new Side(SideType.Front, command.Front.Label, command.Front.Example, this);
        }
        
        if (ShouldBeUpdated(Back, command.Back))
        {
            Back = new Side(SideType.Back, command.Back.Label, command.Back.Example, this);
        }
        
        UpdateDetails(FrontDetails, command.Front);
        UpdateDetails(BackDetails, command.Back);
    }

    public void Remove()
    {
        FrontDetails = null;
        BackDetails = null;
    }

    private bool ShouldBeUpdated(Side side, Commands.Side newSide) =>
        side.Label != newSide.Label || side.Example != newSide.Example;

    private void UpdateDetails(Details details, Commands.Side newSide)
    {
        details.SetQuestionable(newSide.UseAsQuestion);
    }
}