using System.Runtime.InteropServices.JavaScript;
using Domain.Enterprise.models;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Domain.StoreSystem.ValueObjects;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.Application;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockService _stockService;
    private readonly IProductRepository _productRepository;
    private readonly IStockItemRepository _stockItemRepository;

    public OrderService(IOrderRepository orderRepository, IStockService stockService,
        IProductRepository productRepository, IStockItemRepository stockItemRepository)
    {
        _orderRepository = orderRepository;
        _stockService = stockService;
        _productRepository = productRepository;
        _stockItemRepository = stockItemRepository;
    }

  public async Task<Order> PlaceOrderAsync(List<OrderProduct> orderProducts)
{
    var order = new Order
    {
        Id = Guid.NewGuid(),
        OrderSupplier = new List<OrderSupplier>()
    };
    var supplierToOrderSupplierMap = new Dictionary<Guid, OrderSupplier>();

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
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                OrderDate = DateTime.Now,
                DeliveryDate = null,
                Order = order,
                OrderSupplierProducts = new List<OrderSupplierCachedProduct>()
            };
            supplierToOrderSupplierMap[product.SupplierId] = orderSupplier;
            order.OrderSupplier.Add(orderSupplier);

            orderSupplier.OrderSupplierProducts.Add(new OrderSupplierCachedProduct
            {
                Id = Guid.NewGuid(),
                OrderSupplierId = orderSupplier.Id,
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
                    OrderSupplierId = orderSupplier.Id,
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
    return order;
}
    public async Task RollReceivedOrderAsync(Guid orderSupplierId)
    {
        var orderSupplier = await _orderRepository.GetOrderSupplierByIdAsync(orderSupplierId);
        if (orderSupplier == null)
        {
            throw new Exception("Order supplier not found");
        }

        orderSupplier.DeliveryDate = DateTime.Now;
        await _stockService.UpdateStockFromOrderAsync(orderSupplier, false);
    }

    public async Task<List<Order>?> ShowOrders(List<Guid>? orderIds)
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