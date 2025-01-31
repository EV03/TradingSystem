using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IStockItemRepository {
    
    // später rausnehmen
    Task<StockItem?> GetByBarcodeAsync(string barcode);
    
    Task<StockItem?> UpdateAsync(StockItem stockItem);
    
    Task<IEnumerable<StockItem>?> GetAllStocksByStoreIdAsync(Guid storeId);
    
}