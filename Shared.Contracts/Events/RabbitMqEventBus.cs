using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly string? _storeId;

    private const string ExchangeName = "enterprise.events";

    public RabbitMqEventBus(IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceProvider = serviceProvider;

        _enterpriseId = Environment.GetEnvironmentVariable("ENTERPRISE_ID") ?? 
            throw new Exception("ENTERPRISE_ID is not set");
        _storeId = Environment.GetEnvironmentVariable("STORE_ID");

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, true);
    }

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        var eventName = typeof(TEvent).Name.Replace("Event", "").ToLower();
        var routingKey = GetRoutingKey(@event, eventName);
        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(ExchangeName, routingKey, null, body);
        return Task.CompletedTask;
    }

    private string GetRoutingKey<TEvent>(TEvent @event, string eventName) where TEvent : class 
    {
        // store event, format: store.<enterpriseId>.<eventName>
        // enterprise event, format: enterprise.<enterpriseId>.<eventName>
        if (@event is IStoreEvent)
        {
            if (_storeId == null)
                throw new Exception("Store events cannot be published without STORE_ID");
            return $"store.{_enterpriseId}.{eventName}";
        }
        return $"enterprise.{_enterpriseId}.{eventName}";
    }

    public Task SubscribeAsync<TEvent, TEventHandler>() 
        where TEvent : class 
        where TEventHandler : IEventHandler<TEvent>
    {
        var queueName = GetQueueName<TEvent>();
        var routingPattern = GetRoutingPattern<TEvent>();

        _channel.QueueDeclare(queueName, false, false, true, null);
        _channel.QueueBind(queueName, ExchangeName, routingPattern);
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            // explicit scope erstellen da rabbitmq event handler nicht in request scope ist
            // hat mich zu viel zeit gekostet ...
            using (var scope = _serviceProvider.CreateScope()) 
            {
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TEvent>(message);

                if (@event != null && ShouldHandleEvent(@event, ea.RoutingKey))
                {
                    await handler.HandleAsync(@event);  
                }
            }
        };
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
    private string GetQueueName<TEvent>() where TEvent : class
    {
        var prefix = typeof(IStoreEvent).IsAssignableFrom(typeof(TEvent)) ? "store" : "enterprise";
        var suffix = _storeId != null ? $".{_storeId}" : "";  // for enterprise store.enterpriseid.eventname
        return $"{prefix}.{_enterpriseId}{suffix}.{typeof(TEvent).Name}";
    }

    private string GetRoutingPattern<TEvent>() where TEvent : class
    {
        if (typeof(IStoreEvent).IsAssignableFrom(typeof(TEvent)))
        {
            // enterprises / stores subscribe to store events
            return $"store.{_enterpriseId}.{typeof(TEvent).Name.Replace("Event", "").ToLower()}";                       
        }
        
        // store subscribes to enterprise events 
        if (_storeId != null) 
            return $"enterprise.{_enterpriseId}.#";
            
        // enterprise should not subscribe to enterprise events
        return "";
    
    }

    private bool ShouldHandleEvent<TEvent>(TEvent @event, string routingKey) where TEvent : class
    {
        // For enterprise events, only handle if enterprise ID matches
        if (@event is IEnterpriseEvent enterpriseEvent)
        {
            return enterpriseEvent.EnterpriseId.ToString() == _enterpriseId;
        }
        
        // For store events, handle if we're enterprise or a different store
        if (@event is IStoreEvent storeEvent)
        {
            var routingParts = routingKey.Split('.');
            if (routingParts.Length >= 4)
            {
                var sourceStoreId = routingParts[3];
                // Enterprise handles all store events
                // Stores handle events from other stores (not their own)
                return _storeId == null || _storeId != sourceStoreId;
            }
        }
    
        return true;
    }
}
