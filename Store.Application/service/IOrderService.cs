
using Domain.StoreSystem.models;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IOrderService
{
    Task<Order>  PlaceOrderAsync(List<OrderProductDto>  order); 
    Task RollReceivedOrderAsync(Guid orderSupplierId);
    Task<List<Order>?> ShowOrders(List<Guid>? orderIds);
}  