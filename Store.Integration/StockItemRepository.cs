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
        return await _context.StockItems.FindAsync(id);
    }
    public async Task<StockItem?> GetByBarcodeAsync(string barcode)
    {
        return await _context.StockItems.FirstOrDefaultAsync(item => item.Barcode == barcode);
    }
    public async Task<StockItem?> AddAsync(StockItem stockItem)
    {
        _context.StockItems.Add(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }

    public async Task<StockItem> UpdateAsync(StockItem stockItem)
    {
        _context.StockItems.Update(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }

    public async Task DeleteAsync(Guid id)
    {
        var stockItem = await _context.StockItems.FindAsync(id);
        if (stockItem != null)
        {
            _context.StockItems.Remove(stockItem);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<IEnumerable<StockItem>?> GetAllAsync()
    {
        return await _context.StockItems.ToListAsync();
    }

    public async Task<StockItem?> GetByProductIdAndStoreIdAsync(Guid productId, Guid storeId)
    {
        return await _context.StockItems.FirstOrDefaultAsync(item => item.ProductId == productId && item.StoreId == storeId);
    }
}