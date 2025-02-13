using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class LowStockEventHandler : IEventHandler<LowStockEvent>
{
    public Task HandleAsync(LowStockEvent @event)
    {
        throw new NotImplementedException();
    }
}