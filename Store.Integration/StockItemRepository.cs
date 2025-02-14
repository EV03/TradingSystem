using Domain.StoreSystem;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Microsoft.EntityFrameworkCore;

namespace Store.Integration;
public class StockItemRepository : IStockItemRepository
{
    private readonly StoreContext _context;
    
    public StockItemRepository(StoreContext context)
    {
        _context = context;
    }
    public async Task<StockItem?> GetByIdAsync(long id)
    {
        return await _context.StockItems.FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<StockItem?> UpdateAsync(StockItem stockItem)
    {
        _context.StockItems.Update(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }
    
    public async Task<IEnumerable<StockItem>?> GetAllStocksAsync()
    {
        return await _context.StockItems.ToListAsync();
    }

    public Task<StockItem?> GetByCachedProductIdAsync(long cachedProductId)
    {
        return _context.StockItems.FirstOrDefaultAsync(item => item.CachedProductId == cachedProductId);
    }
    
    public async Task<StockItem?> GetByProductIdAsync(long productId)
    {
        return await _context.StockItems.FirstOrDefaultAsync(item => item.CachedProductId == productId);
    }
}