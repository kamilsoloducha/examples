namespace EfCoreDdd.Model.Entities;

public class Owner : Entity, IAggregateRoot
{
    private readonly List<Group> _groups = new();
    public virtual IReadOnlyList<Group> Groups => _groups.AsReadOnly();
    
    public Owner()
    {
    }

    public Group CreateGroup(string name)
    {
        var newGroup = new Group(name, this);
        
        _groups.Add(newGroup);
        
        return newGroup;
    }
}