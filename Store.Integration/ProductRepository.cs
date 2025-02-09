
using CashDesk.Integration;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Microsoft.EntityFrameworkCore;


namespace Store.Integration;
public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;
    
    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<CachedProduct?> GetByProductIdAsync(Guid productId)
    {
        return await _context.CachedProducts
            .Include(product => product.StockItem)
            .FirstOrDefaultAsync(product => product.ProductId == productId);
    }

    public Task<CachedProduct?> GetByIdAsync(Guid id)
    {
     
        return _context.CachedProducts
            .AsNoTracking()
            .Include(product => product.StockItem)
            .FirstOrDefaultAsync(product => product.Id == id);
    }

    public Task<CachedProduct?> GetByBarcodeAsync(string barcode)
    {
        return _context.CachedProducts
            .Include(product => product.StockItem)
            .FirstOrDefaultAsync(product => product.Barcode == barcode);
    }
    
    public async Task<CachedProduct?> Update(CachedProduct product)
    {
        _context.CachedProducts.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<CachedProduct>> GetProductsAsync()
    {
        return await _context.CachedProducts.ToListAsync();
    }
}