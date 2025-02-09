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
    public async Task<StockItem?> GetByIdAsync(Guid id)
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

    public Task<StockItem?> GetByCachedProductIdAsync(Guid cachedProductId)
    {
        return _context.StockItems.FirstOrDefaultAsync(item => item.CachedProductId == cachedProductId);
    }
}