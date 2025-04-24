using System.Diagnostics;
using System.Text;
using EventBus.Abstraction;
using EventBus.Configuration;
using EventBus.ErrorHandling;
using EventBus.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace EventBus;

public class RabbitMqEventBus : IEventBus
{
    private readonly INamingFormatter _namingFormatter;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeProvider _timeProvider;
    private readonly ConnectionFactory _connectionFactory;
    private readonly CancellationTokenSource _applicationStoppingTokenSource;
    private readonly RabbitConfiguration _rabbitConfiguration;
    private IConnection? _connection;

    public RabbitMqEventBus(IOptions<RabbitConfiguration> options,
        INamingFormatter namingFormatter,
        IServiceScopeFactory serviceScopeFactory,
        TimeProvider timeProvider)
    {
        _namingFormatter = namingFormatter;
        _serviceScopeFactory = serviceScopeFactory;
        _timeProvider = timeProvider;
        _connectionFactory = new ConnectionFactory
        {
            HostName = options.Value.Host,
            Port = options.Value.Port,
            Password = options.Value.Password,
            UserName = options.Value.UserName
        };
        _applicationStoppingTokenSource = new CancellationTokenSource();
        _rabbitConfiguration = options.Value;
    }

    #region Connector

    public async Task Connect(CancellationToken cancellationToken = default)
    {
        if (_connection is not null && _connection.IsOpen) return;

        _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
    }

    public async Task Disconnect(CancellationToken cancellationToken = default)
    {
        if (_connection is null || !_connection.IsOpen) return;

        await _applicationStoppingTokenSource.CancelAsync();

        await _connection.CloseAsync(cancellationToken);
        await _connection.DisposeAsync();
    }

    #endregion

    #region Subsciber

    public async Task Subscribe<TEvent>(CancellationToken cancellationToken = default)
    {
        var exchangeName = GetExchangeName<TEvent>();
        var queueName = GetQueueName<TEvent>();

        var channel = await CreateChannel(_connection, cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Fanout,
            true,
            false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queueName,
            exchangeName,
            routingKey: string.Empty,
            arguments: null,
            cancellationToken: cancellationToken);


        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            if (_applicationStoppingTokenSource.IsCancellationRequested) return;

            TEvent? @event;
            var body = Array.Empty<byte>();

            try
            {
                var traceIdValue = string.Empty;
                var props = eventArgs.BasicProperties;
                if (props.Headers is not null &&
                    props.Headers.TryGetValue(BasicPropertiesExtensions.TraceIdHeaderKey, out var traceIdValueObject))
                {
                    if (traceIdValueObject is byte[])
                    {
                        traceIdValue = Encoding.UTF8.GetString((byte[])traceIdValueObject);
                    }
                }

                Activity.Current = new Activity(nameof(IEventBusSubscriber.Subscribe))
                    .SetParentId(
                        ActivityTraceId.CreateFromString(traceIdValue),
                        ActivitySpanId.CreateRandom(),
                        ActivityTraceFlags.Recorded).Start();

                body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                @event = JsonConvert.DeserializeObject<TEvent>(message);

                if (@event is null)
                {
                    throw new InvalidOperationException("Deserialized event cannot be null");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                await MoveToErrorQueue<TEvent>(
                    body,
                    channel,
                    eventArgs,
                    _applicationStoppingTokenSource.Token);
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var eventHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                await eventHandler.Handle(@event, _applicationStoppingTokenSource.Token);
                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
            catch (FatalException ex)
            {
                Log.Fatal(ex, "Error while processing message");

                await MoveToErrorQueue<TEvent>(
                    body,
                    channel,
                    eventArgs,
                    _applicationStoppingTokenSource.Token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                await DelayEvent<TEvent>(body,
                    channel,
                    eventArgs,
                    _applicationStoppingTokenSource.Token);
            }
            finally
            {
                Activity.Current.Dispose();
            }
        };

        await channel.BasicConsumeAsync(
            queueName,
            autoAck: false,
            consumer,
            cancellationToken: _applicationStoppingTokenSource.Token);

        Log.Information("Subscription created - {Exchange} - {Queue}", exchangeName, queueName);
    }

    private async Task MoveToErrorQueue<TEvent>(byte[] body, IChannel channel, BasicDeliverEventArgs ea,
        CancellationToken cancellationToken = default)
    {
        Log.Error("Moving {Event} to error queue", typeof(TEvent).Name);
        
        var errorQueueName = GetErrorQueueName<TEvent>();

        await channel.QueueDeclareAsync(
            errorQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await channel.BasicPublishAsync(
            string.Empty,
            errorQueueName,
            true,
            new BasicProperties(),
            body,
            cancellationToken);

        await channel.BasicAckAsync(
            ea.DeliveryTag,
            multiple: false,
            cancellationToken);
    }

    private async Task DelayEvent<TEvent>(byte[] body, IChannel channel, BasicDeliverEventArgs eventArgs,
        CancellationToken cancellationToken = default)
    {
        TimeSpan delay;
        if (eventArgs.BasicProperties.Headers is not null
            && eventArgs.BasicProperties.Headers.TryGetValue("x-delay", out var delayObject)
            && delayObject is long delayMs)
        {
            delay = delayMs < 0 ? TimeSpan.FromMilliseconds(delayMs) : TimeSpan.FromMilliseconds(-10);
        }
        else
        {
            delay = TimeSpan.FromMilliseconds(-10);
        }
        
        delay *= 2;

        if (delay < _rabbitConfiguration.MaxDelay)
        {
            await MoveToErrorQueue<TEvent>(body, channel, eventArgs, cancellationToken);
            return;
        }

        var props = new BasicProperties()
        {
            Headers = new Dictionary<string, object?>(),
            Persistent = true
        };
        props.AddDateTimeOffset(_timeProvider.GetLocalNow().UtcDateTime);
        props.AddDelay(delay);
        props.AddTraceId(GetTraceId());

        await channel.BasicAckAsync(
            eventArgs.DeliveryTag,
            multiple: false);
        
        Log.Information("Delaying event for {Delay} ms", delay.TotalMilliseconds);
        
        var queueName = GetQueueName<TEvent>();
        var delayExchangeName = "DelayExchange";
        
        await channel.ExchangeDeclareAsync(
            exchange: delayExchangeName,
            type: "x-delayed-message",
            durable: true,
            autoDelete: false,
            arguments: new Dictionary<string, object?> { { "x-delayed-type", ExchangeType.Direct } },
            cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queueName, delayExchangeName, queueName);

        await channel.BasicPublishAsync(
            "DelayExchange",
            GetQueueName<TEvent>(),
            false,
            props,
            body);
    }

    #endregion

    #region Publisher

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        var exchangeName = GetExchangeName<TEvent>();

        var serializedEvent = JsonConvert.SerializeObject(@event);

        var messageBodyBytes = Encoding.UTF8.GetBytes(serializedEvent);
        var props = new BasicProperties()
        {
            Headers = new Dictionary<string, object?>()
        };
        props.AddDateTimeOffset(_timeProvider.GetLocalNow().UtcDateTime);
        props.AddTraceId(GetTraceId());

        var channel = await CreateChannel(_connection, cancellationToken);
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, true, false,
            cancellationToken: cancellationToken);
        await channel.BasicPublishAsync(exchangeName, string.Empty, false, props, messageBodyBytes, cancellationToken);
    }

    #endregion


    private string GetQueueName<TEvent>() => _namingFormatter.QueueName(typeof(TEvent).Name);
    private string GetExchangeName<TEvent>() => _namingFormatter.ExchangeName(typeof(TEvent).Name);
    private string GetErrorQueueName<TEvent>() => _namingFormatter.ErrorQueueName(typeof(TEvent).Name);

    private static string GetTraceId() => Activity.Current is not null
        ? Activity.Current.TraceId.ToHexString()
        : new Activity(nameof(RabbitMqEventBus)).TraceId.ToHexString();

    private static Task<IChannel> CreateChannel(IConnection? connection, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);
    }
}