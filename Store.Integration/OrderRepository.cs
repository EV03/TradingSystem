using Domain.StoreSystem;
using Domain.StoreSystem.repository;
using Domain.StoreSystem.models;
using Microsoft.EntityFrameworkCore;

namespace Store.Integration;
public class OrderRepository : IOrderRepository
{
    private readonly StoreContext _context;

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<Order?> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>?> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<OrderSupplier?> GetOrderSupplierByIdAsync(Guid orderSupplierId)
    {
        return await _context.OrderSuppliers.FirstOrDefaultAsync(orderSupplier => orderSupplier.Id == orderSupplierId);
    }

    public async Task<OrderSupplier?> AddOrderSupplierAsync(OrderSupplier orderSupplier)
    {
       await _context.OrderSuppliers.AddAsync(orderSupplier);
       await _context.SaveChangesAsync();
       return orderSupplier;
    }

    public Task UpdateOrderSupplierAsync(OrderSupplier orderSupplier)
    {
        _context.OrderSuppliers.Update(orderSupplier);
        return _context.SaveChangesAsync();
    }
}