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

    public async Task<IEnumerable<Order>?> GetByStoreIdAndOrderId(Guid storeId, Guid orderId)
    {
        return await _context.Orders.Where(order => order.StoreId == storeId && order.Id == orderId).ToListAsync();
    }
    
}