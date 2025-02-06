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
    
    public RabbitMqEventBus(IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceProvider = serviceProvider;
    }
    
    
    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        var eventName = typeof(TEvent).Name;
        _channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish("", eventName, null, body);
        return Task.CompletedTask;
    }

    public Task SubscribeAsync<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        var eventName = typeof(TEvent).Name;
        _channel.QueueDeclare(queue: eventName, durable: false, exclusive: false, autoDelete: false, arguments: null);

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

        _channel.BasicConsume(queue: eventName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
