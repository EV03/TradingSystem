
using Domain.StoreSystem.models;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    Task<List<Order>>  PlaceOrderAsync(OrderDto order); 
    Task RollReceivedOrderAsync(Guid orderSupplierId);
    Task<List<OrderSupplier>> ShowOrders(List<Guid?> orderIds);
}  