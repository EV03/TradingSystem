using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> AddAsync(Order order);
    Task<OrderItem?> SaveOrderItemAsync(OrderItem orderItem);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Order>?> GetAll();
    Task<IEnumerable<Order>?> GetByStoreId(Guid storeId);
    Task<IEnumerable<Order>?> GetByStoreIdAndStatus(Guid storeId, string status);
    
    
}