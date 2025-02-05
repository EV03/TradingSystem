
using CashDesk.Integration;
using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Microsoft.EntityFrameworkCore;


namespace Store.Integration;
public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public async Task<CachedProduct?> GetByProductIdAsync(Guid id)
    {
        return await _context.CachedProducts.FirstOrDefaultAsync(product => product.ProductId == id);
    }

    public Task<CachedProduct?> GetByBarcodeAsync(string barcode)
    {
        return _context.CachedProducts.FirstOrDefaultAsync(product => product.Barcode == barcode);
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