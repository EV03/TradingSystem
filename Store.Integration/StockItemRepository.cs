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
    // später rausnehmen
    public async Task<StockItem?> GetByBarcodeAsync(string barcode)
    {
        return await _context.StockItems.FirstOrDefaultAsync(item => item.Barcode == barcode);
    }

    public async Task<StockItem?> UpdateAsync(StockItem stockItem)
    {
        _context.StockItems.Update(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }
    
    public async Task<IEnumerable<StockItem>?> GetAllStocksByStoreIdAsync(Guid storeId)
    {
        return await _context.StockItems.Where(item => item.StoreId == storeId).ToListAsync();
    }
}