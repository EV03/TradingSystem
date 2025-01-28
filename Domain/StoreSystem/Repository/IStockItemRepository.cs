using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IStockItemRepository
{
    Task<StockItem> GetByIdAsync(Guid id);
    
    Task<StockItem> GetByBarcodeAsync(string barcode);
    Task<StockItem> SaveAsync(StockItem stockItem);
    
    Task<StockItem> UpdateAsync(StockItem stockItem);
    
    Task DeleteAsync(Guid id);
    
    IEnumerable<StockItem> GetAllasync();
    
    Task<StockItem> GetByProductIdAndStoreIdAsync(Guid productId, Guid storeId);
    
    // get by high stock products >= min stock
    
    // get by low stock products < min stock
    
    
}