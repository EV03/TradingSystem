
using Domain.StoreSystem.models;
using Domain.StoreSystem.ValueObjects;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    Task<Order>  PlaceOrderAsync(List<OrderProduct>  orderProducts); 
    Task RollReceivedOrderAsync(Guid orderSupplierId);
    Task<List<Order>?> ShowOrders(List<Guid>? orderIds);
}  