using Domain.StoreSystem;
using Domain.StoreSystem.repository;
using Domain.StoreSystem.models;
using Microsoft.EntityFrameworkCore;

namespace Store.Integration;

public class OrderRepository : IOrderRepository
{
    private readonly StoreContext _context;

    public OrderRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order?> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<OrderItem?> SaveOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Order>?> GetAll()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>?> GetByStoreId(Guid storeId)
    {
        return await _context.Orders.Where(order => order.StoreId == storeId).ToListAsync();
    }

    public async Task<IEnumerable<Order>?> GetByStoreIdAndStatus(Guid storeId, string status)
    {
        return await _context.Orders.Where(order => order.StoreId == storeId && order.Status == status).ToListAsync();
    }
}