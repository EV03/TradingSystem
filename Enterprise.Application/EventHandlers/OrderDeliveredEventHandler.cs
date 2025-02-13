using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class OrderDeliveredEventHandler : IEventHandler<OrderDeliveredEvent>
{
    public Task HandleAsync(OrderDeliveredEvent @event)
    {
        throw new NotImplementedException();
    }
}