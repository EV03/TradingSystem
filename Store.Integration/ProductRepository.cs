
using CashDesk.Integration;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Microsoft.EntityFrameworkCore;


namespace Store.Integration;
public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public async Task<CachedProduct?> GetByIdAsync(Guid id)
    {
        return await _context.CachedProducts.FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<CachedProduct?> GetByBarcodeAsync(string barcode, Guid storeId)
    {
        return await _context.CachedProducts.FirstOrDefaultAsync(product => product.Barcode == barcode && product.StoreId == storeId);
    }

    public async Task UpdatePriceAsync(Guid stockItemId, float newPrice)
    { 
        var stockItem = await _context.StockItems.FirstOrDefaultAsync(stockItem => stockItem.Id == stockItemId);
        if (stockItem != null)
        {
            stockItem.SalesPrice = newPrice;
            await _context.SaveChangesAsync();
        }
    }
}