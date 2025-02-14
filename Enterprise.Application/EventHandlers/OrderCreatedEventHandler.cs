using Domain.Enterprise.models;
using Enterprise.Application.Services;
using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IOrderProcessingService _orderProcessingService;
    
    public OrderCreatedEventHandler(IOrderProcessingService orderProcessingService)
    {
        _orderProcessingService = orderProcessingService;
    }
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        var deliveryLog = new DeliveryLog()
        {
            OrderId = @event.OrderId,
            OrderSupplierId = @event.OrderSupplierId,
            SupplierId = @event.SupplierId,
            SupplierName = @event.SupplierName ?? "unknown",
            OrderDate = @event.OrderDate,
            DeliveryDate = null
        };
        await _orderProcessingService.ProcessOrdersAsync(deliveryLog);
    }
}