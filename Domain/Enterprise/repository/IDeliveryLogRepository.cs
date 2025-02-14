using Domain.Enterprise.models;
using Domain.Enterprise.ValueObjects;

namespace Domain.Enterprise.repository;

public interface IDeliveryLogRepository
{
    public Task<List<SupplierDeliveryTime>> GetAverageSupplierDeliveryTimes();
    
    public Task<DeliveryLog> AddDeliveryLogAsync(DeliveryLog deliveryLog);
    
    public Task<DeliveryLog?> GetDeliveryLogByOrderSupplierIdAsync(long id);
    
    public Task UpdateAsync(DeliveryLog deliveryLog);
}