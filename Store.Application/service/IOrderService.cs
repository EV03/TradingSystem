using Domain.StoreSystem;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    // Places orders for a store grouped by supplier 
    Task<List<Order>>  PlaceOrderAsync(OrderDto order, Guid storeId); 
    
    Task RollReceivedOrderAsync(Guid orderId, Guid storeId);
}  