using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<Order?> AddAsync(Order order);
    Task<OrderSupplier> UpdateOrderSupplierAsync(OrderSupplier orderSupplier);
    Task<List<Order>?> GetAllOrdersAsync();
    Task<OrderSupplier?> GetOrderSupplierByIdAsync(Guid orderSupplierId);
    
}