using Domain.Enterprise.models;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.Application;

public class OrderService : IOrderService 
{
    private readonly IOrderRepository _orderRepository; 
    private readonly IStockItemRepository _stockItemRepository; 
    
    public OrderService(IOrderRepository orderRepository, IStockItemRepository stockItemRepository)
    {
        _orderRepository = orderRepository;
        _stockItemRepository = stockItemRepository;
    }

    public async Task<List<Order>> PlaceOrderAsync(OrderDto orderDto, Guid storeId)
    {
        var orders = new List<Order>();
        // supplier id -> order items
        var supplierOrderItems = new Dictionary<Guid, List<OrderItem>>();


        foreach (var orderItemDto in orderDto.OrderItems)
        {
            // var product = await GetProductFromEnterpriseServer(orderItemDto.ProductId);
            var product = new Product(); // mock product

            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                Amount = orderItemDto.Amount,
                ProductId = orderItemDto.ProductId,
                OrderId = Guid.Empty // will be updated later
            };

            // Group OrderItems by Supplier and create an Order for each Supplier
            if (!supplierOrderItems.ContainsKey(Product.SupplierId))
            {
                supplierOrderItems[Product.SupplierId] = new List<OrderItem>();
            }

            supplierOrderItems[Product.SupplierId].Add(orderItem);

        }
        
        // Create an Order for each Supplier
        foreach (var supplierOrderItem in supplierOrderItems)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                OrderItems = supplierOrderItem.Value,
                OrderingDate = DateTime.Now,
                Status = "incoming"
            };
            
            // update order id in order items
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = order.Id;
            }
            
            orders.Add(order);
            await _orderRepository.AddAsync(order);
        }
        return orders;
    }
    
    public async Task RollReceivedOrderAsync(Guid orderId, Guid storeId)
    {
       var order = await _orderRepository.GetByIdAsync(orderId);
       
       if(order.StoreId != storeId)
       {
           throw new UnauthorizedAccessException("You do not have permission to roll this order.");
       }
       order.Status = "arrived";
       order.DeliveryDate = DateTime.Now;
       // update stock 
         foreach (var orderItem in order.OrderItems)
         {
           // var product = await GetProductFromEnterpriseServer(orderItem.ProductId);
              var product = new Product(); // mock product
              var stockItem = await _stockItemRepository.GetByProductIdAndStoreIdAsync(product.Id, order.StoreId);
              stockItem.AvailableQuantity += orderItem.Amount;
                
              await _stockItemRepository.UpdateAsync(stockItem);
         }
       
    }
} 