using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Group : Entity, IAggregateRoot
{
    public GroupName Name { get; }
    public virtual Owner Owner { get; private set; }
    

    private readonly List<Card> _cards = new();
    public virtual IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    protected Group()
    {
    }

    public Group(GroupName name, Owner owner) : this()
    {
        Name = name;
        Owner = owner;
    }

    public Card AddCard(Label front, Label back, Example frontExample, Example backExample)
    {
        var newCard = new Card(front, back, frontExample, backExample, this);
        _cards.Add(newCard);
        return newCard;
    }

    public void Remove()
    {
        _cards.ForEach(x => x.Remove());
        
        Owner = null;
    }
}