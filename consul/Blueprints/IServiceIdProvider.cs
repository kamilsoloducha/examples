namespace Blueprints;

public interface IServiceIdProvider<T>
{
    T GetId();
}