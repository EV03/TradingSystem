using Domain.Enterprise.models;

namespace Enterprise.Application.Services;

public interface IOrderProcessingService
{
    Task ProcessOrdersAsync(DeliveryLog deliveryLog);
    
    Task UpdateDeliveryLogAsync(long OrderSupplierId, DateTime deliveryDate);
}