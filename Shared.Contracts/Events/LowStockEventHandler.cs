namespace Shared.Contracts.Events;

public class LowStockEventHandler : IEventHandler<LowStockEvent>
{

    public Task HandleAsync(LowStockEvent @event)
    {
        throw new NotImplementedException();
    }
}