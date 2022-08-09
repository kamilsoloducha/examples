using Consul;
using Consul.Filtering;

namespace Blueprints.Internal;

internal class ConsulUrlProvider : IServiceDiscoveryUrlProvider
{
    private readonly IConsulClient _consulClient;

    public ConsulUrlProvider(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    public async Task<Uri> GetUrls(string serviceName, string absolutePath, CancellationToken cancellationToken)
    {
        const string filterField = "Service";
        var serviceNameSelector = new StringFieldSelector(filterField) == serviceName;
        var allServices = await _consulClient.Agent.Services(serviceNameSelector, CancellationToken.None);

        var randomService = GetRandomInstance(allServices.Response.Values.ToList());

        var uriBuilder = new UriBuilder
        {
            Host = randomService.Address,
            Port = randomService.Port,
            Path = absolutePath
        };
        return uriBuilder.Uri;
    }
    

    private AgentService GetRandomInstance(IList<AgentService> services)
        => services[Random.Shared.Next(0, services.Count)];
}