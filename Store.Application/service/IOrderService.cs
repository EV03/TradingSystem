
using Domain.StoreSystem.models;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    Task<List<Order>>  PlaceOrderAsync(OrderDto order, Guid storeId); 
    Task RollReceivedOrderAsync(Guid orderSupplierId);
    Task<List<Order>> ShowOrders(Guid storeId, List<Guid?> orderIds);
}  