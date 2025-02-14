using Domain.Enterprise.models;
using Domain.Enterprise.repository;
using Enterprise.Application.Services;

namespace Enterprise.Application;

public class OrderProcessingService : IOrderProcessingService
{
    private readonly IDeliveryLogRepository _deliveryLogRepository;

    public OrderProcessingService(IDeliveryLogRepository deliveryLogRepository)
    {
        _deliveryLogRepository = deliveryLogRepository;
    }
    public async Task ProcessOrdersAsync(DeliveryLog deliveryLog)
    {
        Console.WriteLine("Processing order");
        await _deliveryLogRepository.AddDeliveryLogAsync(deliveryLog);
    }

    public async Task UpdateDeliveryLogAsync(long orderSupplierId, DateTime deliveryDate)
    {
        var deliveryLog = await _deliveryLogRepository.GetDeliveryLogByOrderSupplierIdAsync(orderSupplierId);
        if (deliveryLog == null)
        {
            throw new KeyNotFoundException($"DeliveryLog not found for OrderSupplierId: {orderSupplierId}");
        }
        Console.WriteLine("Updating delivery log");
        deliveryLog.DeliveryDate = deliveryDate;
        await _deliveryLogRepository.UpdateAsync(deliveryLog);
    }
}