using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Blueprints.RabbitMassTransit;

public class BusService : IHostedService
{
    private readonly IBusControl busControl;

    public BusService(IBusControl busControl)
    {
        this.busControl = busControl;
    }

    public async Task StartAsync(CancellationToken cancellationToken) => await busControl.StartAsync(cancellationToken);

    public async Task StopAsync(CancellationToken cancellationToken) => await busControl.StopAsync(cancellationToken);
}