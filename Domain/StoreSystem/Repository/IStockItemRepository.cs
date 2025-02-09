using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IStockItemRepository {
    
    Task<StockItem?> GetByIdAsync(Guid id);
    
    Task<StockItem?> UpdateAsync(StockItem stockItem);
    
    Task<IEnumerable<StockItem>?> GetAllStocksAsync();
    
    Task<StockItem?> GetByCachedProductIdAsync(Guid cachedProductId);
    
}