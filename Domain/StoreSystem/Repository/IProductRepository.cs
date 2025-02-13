using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IProductRepository
{
   Task<CachedProduct?> GetByProductIdAsync(long productId);
    Task<CachedProduct?> GetByIdAsync(long id);
   Task<CachedProduct?> GetByBarcodeAsync(string barcode);
   Task<CachedProduct?> Update(CachedProduct product);
   Task<List<CachedProduct>> GetProductsAsync();
}