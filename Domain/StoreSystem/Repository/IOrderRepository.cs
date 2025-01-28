namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id);
    Task SaveAsync(Order order);
    Task SaveOrderItemAsync(OrderItem orderItem);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
    IEnumerable<Order> GetAll();
    IEnumerable<Order> GetByStoreId(Guid storeId);
    IEnumerable<Order> GetByStoreIdAndStatus(Guid storeId, string status);
    
    
}