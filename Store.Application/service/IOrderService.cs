
using Domain.StoreSystem.models;
using Domain.StoreSystem.ValueObjects;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    Task<Order>  PlaceOrderAsync(List<OrderProduct>  orderProducts); 
    Task RollReceivedOrderAsync(long orderSupplierId);
    Task<List<Order>?> ShowOrders(List<long>? orderIds);
}  