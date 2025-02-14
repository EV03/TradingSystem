namespace Shared.Contracts.Events;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
    Task SubscribeAsync<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>;
}