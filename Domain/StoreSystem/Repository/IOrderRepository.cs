using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task<IEnumerable<Order>?> GetByStoreIdAndOrderId(Guid storeId, Guid orderId);
    
}