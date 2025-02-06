using Shared.Contracts.Events;

namespace Shared.Contracts.Interfaces;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
    Task SubscribeAsync<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>;
}