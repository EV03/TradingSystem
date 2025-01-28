namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id);
    Task<Order> SaveAsync(Order order);
    Task<OrderItem> SaveOrderItemAsync(OrderItem orderItem);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Guid id);
    IEnumerable<Order> GetAll();
    IEnumerable<Order> GetByStoreId(Guid storeId);
    IEnumerable<Order> GetByStoreIdAndStatus(Guid storeId, string status);
    
    
}