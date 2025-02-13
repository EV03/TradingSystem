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

    public async Task<Order?> GetByIdAsync(long id)
    {
          return await _context.Orders
        .Include(o => o.OrderSupplier)
        .ThenInclude(os => os.OrderSupplierProducts)
        .ThenInclude(osp => osp.CachedProduct)
        .FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<Order?> AddAsync(Order order)
    {
        foreach (var orderSupplier in order.OrderSupplier)
        {
            foreach (var orderSupplierProduct in orderSupplier.OrderSupplierProducts)
            {
                _context.Entry(orderSupplierProduct.CachedProduct).State = EntityState.Unchanged;
            }
        }
        
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
        return await _context.Orders.
             Include(order => order.OrderSupplier)
            .ThenInclude(orderSupplier => orderSupplier.OrderSupplierProducts)
            .ThenInclude(orderSupplierProduct => orderSupplierProduct.CachedProduct)
            .ToListAsync();
    }

    public async Task<OrderSupplier?> GetOrderSupplierByIdAsync(long orderSupplierId)
    {
        return await _context.OrderSuppliers.
            Include(orderSupplier => orderSupplier.OrderSupplierProducts)
            .ThenInclude(orderSupplierProduct => orderSupplierProduct.CachedProduct)
            .FirstOrDefaultAsync(orderSupplier => orderSupplier.Id == orderSupplierId);
    }

    public async Task<OrderSupplier?> AddOrderSupplierAsync(OrderSupplier orderSupplier)
    {
       await _context.OrderSuppliers.AddAsync(orderSupplier);
       await _context.SaveChangesAsync();
       return orderSupplier;
    }

    public async Task<OrderSupplier> UpdateOrderSupplierAsync(OrderSupplier orderSupplier)
    {
        _context.OrderSuppliers.Update(orderSupplier); 
        await  _context.SaveChangesAsync();
        return orderSupplier;
    }
}