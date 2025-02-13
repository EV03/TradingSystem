using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    public Task HandleAsync(OrderCreatedEvent @event)
    {
        throw new NotImplementedException();
    }
}