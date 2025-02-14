using Enterprise.Application.Services;
using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class OrderDeliveredEventHandler : IEventHandler<OrderDeliveredEvent>
{
    private readonly IOrderProcessingService _orderProcessingService;
    public OrderDeliveredEventHandler(IOrderProcessingService orderProcessingService)
    {
        _orderProcessingService = orderProcessingService;
    }
    public Task HandleAsync(OrderDeliveredEvent @event)
    {
        return _orderProcessingService.UpdateDeliveryLogAsync(@event.OrderSupplierId, @event.DeliveryDate);
    }
}