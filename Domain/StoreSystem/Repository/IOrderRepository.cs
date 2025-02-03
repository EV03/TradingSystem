using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<Order?> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task<IEnumerable<Order>?> GetByOrderId( Guid orderId);
    
    Task<OrderSupplier?> GetOrderSupplierByIdAsync(Guid orderSupplierId);
    Task<OrderSupplier?> AddOrderSupplierAsync(OrderSupplier orderSupplier);
    Task UpdateOrderSupplierAsync(OrderSupplier orderSupplier);
    
}