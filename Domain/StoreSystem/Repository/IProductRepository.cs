using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IProductRepository
{
   Task<CachedProduct?> GetByIdAsync(Guid id);
   Task<CachedProduct?> GetByBarcodeAsync(string barcode, Guid storeId);
   Task UpdatePriceAsync(Guid stockItemId, float newPrice);
}