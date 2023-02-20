using EfCoreDdd.Model.ValueObjects;

namespace EfCoreDdd.Model.Entities;

public class Owner : Entity, IAggregateRoot
{
    private readonly List<Group> _groups = new();
    public Guid UserId { get; }
    public virtual IReadOnlyList<Group> Groups => _groups.AsReadOnly();

    protected Owner()
    {
    }

    public Owner(Guid userId):this()
    {
        UserId = userId;
    }

    public Group CreateGroup(GroupName name)
    {
        var newGroup = new Group(name, this);
        
        _groups.Add(newGroup);
        
        return newGroup;
    }

    internal void RemoveGroup(Group group) => _groups.Remove(group);
}