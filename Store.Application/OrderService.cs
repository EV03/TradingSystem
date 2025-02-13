using System.Runtime.InteropServices.JavaScript;
using Domain.Enterprise.models;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Domain.StoreSystem.ValueObjects;
using Shared.Contracts.Dtos;
using Store.Application.service;
using Shared.Contracts.Events;
using Microsoft.Extensions.Configuration;

namespace Store.Application;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockService _stockService;
    private readonly IProductRepository _productRepository;
    private readonly IStockItemRepository _stockItemRepository;
    private readonly IEventBus _eventBus;
    private readonly string _enterpriseId;


    public OrderService(IOrderRepository orderRepository, IStockService stockService,
        IProductRepository productRepository, IStockItemRepository stockItemRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _stockService = stockService;
        _productRepository = productRepository;
        _stockItemRepository = stockItemRepository;
        _eventBus = eventBus;
        _enterpriseId = Environment.GetEnvironmentVariable("ENTERPRISE_ID") ?? throw new Exception("ENTERPRISE_ID is not set");
    }

  public async Task<Order> PlaceOrderAsync(List<OrderProduct> orderProducts)
  {
    var order = new Order
    {
        OrderSupplier = new List<OrderSupplier>()
    };
    var supplierToOrderSupplierMap = new Dictionary<long, OrderSupplier>();

    foreach (var orderProduct in orderProducts)
    {
        var product = await _productRepository.GetByIdAsync(orderProduct.CachedProductId);
        if (product == null)
        {
            Console.WriteLine("Product not found");
            throw new Exception("Product not found");
        }

        if (!supplierToOrderSupplierMap.TryGetValue(product.SupplierId, out var orderSupplier))
        {
            orderSupplier = new OrderSupplier
            {
                SupplierId = product.SupplierId,
                OrderDate = DateTime.Now,
                DeliveryDate = null,
                Order = order,
                OrderSupplierProducts = new List<OrderSupplierCachedProduct>()
            };
            supplierToOrderSupplierMap[product.SupplierId] = orderSupplier;
            order.OrderSupplier.Add(orderSupplier);

            orderSupplier.OrderSupplierProducts.Add(new OrderSupplierCachedProduct
            {
                OrderSupplier = orderSupplier,
                CachedProduct = product,
                CachedProductId = product.Id,
                Quantity = orderProduct.Quantity
            });
        }
        else
        {
            var existingProduct = orderSupplier.OrderSupplierProducts
                .FirstOrDefault(x => x.CachedProductId == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Quantity += orderProduct.Quantity;
            }
            else
            {
                orderSupplier.OrderSupplierProducts.Add(new OrderSupplierCachedProduct
                {
                    OrderSupplier = orderSupplier,
                    CachedProductId = product.Id,
                    CachedProduct = product,
                    Quantity = orderProduct.Quantity
                });
            }
        }
        var stockItem = await _stockItemRepository.GetByCachedProductIdAsync(product.Id);
        if (stockItem == null)
        {
        
            throw new Exception("Stock item not found");
        }
        stockItem.IncomingQuantity += orderProduct.Quantity;
        await _stockItemRepository.UpdateAsync(stockItem);
    }

    await _orderRepository.AddAsync(order);

    foreach (var orderSupplier in supplierToOrderSupplierMap.Values) 
    {
        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            SupplierId = orderSupplier.SupplierId,
            SupplierName = null, // add name later 
            EnterpriseId = long.Parse(_enterpriseId),
            OrderDate = orderSupplier.OrderDate ?? DateTime.Now
        };
        await _eventBus.PublishAsync("order.created",orderCreatedEvent);
    }
    return order;
}
    public async Task RollReceivedOrderAsync(long orderSupplierId)
    {
        var orderSupplier = await _orderRepository.GetOrderSupplierByIdAsync(orderSupplierId);
        if (orderSupplier == null)
        {
            throw new Exception("Order supplier not found");
        }

        if( orderSupplier.DeliveryDate != null)
        {
            throw new Exception("Order already received");
        }
        orderSupplier.DeliveryDate = DateTime.Now;
        await _stockService.UpdateStockFromOrderAsync(orderSupplier, false);
        await _orderRepository.UpdateOrderSupplierAsync(orderSupplier);

        var orderDeliveredEvent = new OrderDeliveredEvent
        {
            OrderId = orderSupplier.OrderId,
            SupplierId = orderSupplier.SupplierId,
            DeliveryDate = orderSupplier.DeliveryDate ?? DateTime.Now,
            EnterpriseId = long.Parse(_enterpriseId)
        };
        await _eventBus.PublishAsync("order.delivered", orderDeliveredEvent);
    }

    public async Task<List<Order>?> ShowOrders(List<long>? orderIds)
    {
        if (orderIds == null || !orderIds.Any())
        {
            // Retrieve all order
            return await _orderRepository.GetAllOrdersAsync();
        }

        var orders = new List<Order>();
        foreach (var orderId in orderIds)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                orders.Add(order);
            }
        }
        return orders;
    }
}