using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IModel = RabbitMQ.Client.IModel;

namespace Shared.Contracts.Events;

public class RabbitMqEventBus : IEventBus
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _enterpriseId;

    private const string ExchangeName = "enterprise.events";
    public RabbitMqEventBus(IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceProvider = serviceProvider;

        _enterpriseId = Environment.GetEnvironmentVariable("ENTERPRISE_ID") ?? throw new Exception("ENTERPRISE_ID is not set");

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
    }
    
    
    public Task PublishAsync<TEvent>(string eventName, TEvent @event) where TEvent : class
    {
        var routingKey = GetRoutingKey(@event, eventName);
        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(ExchangeName, routingKey, null, body);
        return Task.CompletedTask;
    }

    private string GetRoutingKey<TEvent>(TEvent @event, string eventName) where TEvent : class {
        var prefix = @event is IStoreEvent ? "store" : "enterprise";
        return $"{prefix}.{_enterpriseId}.{eventName}";
    }

    public Task SubscribeAsync<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
      var queueName = $"{_enterpriseId}.{typeof(TEvent).Name}";
        var prefix = typeof(IStoreEvent).IsAssignableFrom(typeof(TEvent)) ? "store" : "enterprise";
        var routingPattern = $"{prefix}.{_enterpriseId}.*";

        _channel.QueueDeclare(queueName, false, false, false, null);
        _channel.QueueBind(queueName, ExchangeName, routingPattern);
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<TEvent>(message);

            var handler = (TEventHandler)_serviceProvider.GetService(typeof(TEventHandler));
            if (handler != null)
            {
                await handler.HandleAsync(@event);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
