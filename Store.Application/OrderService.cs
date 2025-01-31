using Domain.Enterprise.models;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.Application;

public class OrderService : IOrderService 
{
    private readonly IOrderRepository _orderRepository; 
    private readonly IStockService _stockService;
    
    public OrderService(IOrderRepository orderRepository, IStockService stockItemRepository)
    {
        _orderRepository = orderRepository;
        _stockService = stockItemRepository;
    }
    
    public Task<List<Order>> PlaceOrderAsync(OrderDto order, Guid storeId)
    {
        throw new NotImplementedException();
    }
    
    public Task RollReceivedOrderAsync(Guid orderSupplierId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> ShowOrders(Guid storeId, List<Guid?> orderIds)
    {
        throw new NotImplementedException();
    }
} 