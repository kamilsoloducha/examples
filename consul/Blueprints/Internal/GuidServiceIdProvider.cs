namespace Blueprints.Internal;

internal class GuidServiceIdProvider : IServiceIdProvider<Guid>
{
    private static readonly Guid GlobalServiceId;
    
    static GuidServiceIdProvider()
    {
        GlobalServiceId = Guid.NewGuid();
    }
    
    public Guid GetId()
    {
        return GlobalServiceId;
    }
}