namespace Blueprints;

public interface IServiceDiscoveryUrlProvider
{
    Task<Uri> GetUrls(string serviceName, string absolutePath, CancellationToken cancellationToken);
}