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
    
    public Task<List<Order>> PlaceOrderAsync(OrderDto order)
    {
        throw new NotImplementedException();
    }
    
    public async Task RollReceivedOrderAsync(Guid orderSupplierId)
    {
        var orderSupplier = await _orderRepository.GetOrderSupplierByIdAsync(orderSupplierId);
        if(orderSupplier == null)
        {
            throw new Exception("Order supplier not found");
        }
        orderSupplier.DeliveryDate = DateTime.Now;
        await _stockService.UpdateStockFromOrderAsync(orderSupplier, false);
    }

    public Task<List<OrderSupplier>> ShowOrders(List<Guid?> orderIds)
    {
        throw new NotImplementedException();
    }
} 