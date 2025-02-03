using System.Runtime.InteropServices.JavaScript;
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
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IStockService stockItemRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _stockService = stockItemRepository;
        _productRepository = productRepository;
    }

    public async Task<Order> PlaceOrderAsync(List<OrderProductDto> orderProductDto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderSupplier = new List<OrderSupplier>()
        };
        // mapped ein supplier zu einem orderSupplier 
        var supplierToOrderSupplierMap = new Dictionary<Guid, Guid>();

        foreach (var orderProduct in orderProductDto)
        {
            var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            OrderSupplier? orderSupplier;
            if (!supplierToOrderSupplierMap.ContainsKey(product.SupplierId))
            {
                supplierToOrderSupplierMap.Add(product.SupplierId, Guid.NewGuid());
                orderSupplier = new OrderSupplier()
                {
                    Id = supplierToOrderSupplierMap[product.SupplierId],
                    OrderId = order.Id,
                    OrderSupplierProducts = new List<OrderSupplierCachedProduct>()
                };

                order.OrderSupplier.Add(orderSupplier);
            }

            var orderSupplierId = supplierToOrderSupplierMap[product.SupplierId];
            orderSupplier = order.OrderSupplier.FirstOrDefault(x => x.Id == orderSupplierId);
            if (orderSupplier == null)
            {
                throw new Exception("Order supplier not found");
            }

            var exisingProduct =
                orderSupplier.OrderSupplierProducts.FirstOrDefault(x => x.CachedProductId == product.Id);
            if (exisingProduct != null)
            {
                exisingProduct.Quantity += orderProduct.Quantity;
                continue;
            }
            else
            {
                orderSupplier.OrderSupplierProducts.Add(new OrderSupplierCachedProduct()
                {
                    OrderSupplierId = orderSupplier.Id,
                    CachedProductId = product.Id,
                    Quantity = orderProduct.Quantity
                });
            }
        }

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
            // Retrieve all orders
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