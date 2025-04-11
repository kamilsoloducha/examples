using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blueprints.RabbitClient.Abstraction;
using Blueprints.RabbitClient.Configuration;
using Blueprints.RabbitClient.ErrorHandling;
using Blueprints.RabbitClient.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using LogContext = Serilog.Context.LogContext;

namespace Blueprints.RabbitClient;

public class RabbitMqEventBus : IEventBus
{
    private readonly INamingFormatter _namingFormatter;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeProvider _timeProvider;
    private IConnection _connection;
    private readonly ConnectionFactory _connectionFactory;
    private readonly CancellationTokenSource _applicationStoppingTokenSource;

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

        var channel = await _connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchangeName,
            ExchangeType.Direct,
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
            
            TEvent @event;
            var body = Array.Empty<byte>();

            try
            {
                string traceIdValue = default;
                var props = eventArgs.BasicProperties;
                if (props.Headers is not null &&
                    props.Headers.TryGetValue(BasicPropertiesExtensions.TraceIdHeaderName, out var traceIdValueBytes))
                {
                    if (traceIdValueBytes is byte[])
                    {
                        traceIdValue = Encoding.UTF8.GetString(traceIdValueBytes as byte[]);
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
            }
            catch (Exception ex)
            {
                await MoveToErrorQueue<TEvent>(body, channel, eventArgs, new FatalException(ex), _applicationStoppingTokenSource.Token);
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var eventHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                await eventHandler.Handle(@event,  _applicationStoppingTokenSource.Token);
                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
            catch (FatalException ex)
            {
                await MoveToErrorQueue<TEvent>(body, channel, eventArgs, ex,  _applicationStoppingTokenSource.Token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                await channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);
            }
            finally
            {
                Activity.Current.Dispose();
            }
        };

        await channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken:  _applicationStoppingTokenSource.Token);

        Log.Information("Subscription created - {Exchange} - {Queue}", exchangeName, queueName);
    }

    private async Task MoveToErrorQueue<TEvent>(byte[] body, IChannel channel, BasicDeliverEventArgs ea,
        FatalException ex, CancellationToken cancellationToken = default)
    {
        Log.Fatal(ex, "Error while processing message");

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

        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
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
            Headers = new Dictionary<string, object>()
        };
        props.AddDateTimeOffset(_timeProvider.GetLocalNow().UtcDateTime);

        var traceId = Activity.Current is not null
            ? Activity.Current.TraceId.ToHexString()
            : new Activity(nameof(IPublishEndpoint.Publish)).TraceId.ToHexString();

        props.AddTraceId(traceId);

        var channel = await _connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);

        await channel.BasicPublishAsync(exchangeName, string.Empty, false, props, messageBodyBytes);
    }

    #endregion

    private string GetQueueName<TEvent>() => _namingFormatter.QueueName(typeof(TEvent).Name);
    private string GetExchangeName<TEvent>() => _namingFormatter.ExchangeName(typeof(TEvent).Name);
    private string GetErrorQueueName<TEvent>() => _namingFormatter.ErrorQueueName(typeof(TEvent).Name);
}