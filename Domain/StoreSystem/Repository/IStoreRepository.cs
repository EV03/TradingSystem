using Domain.StoreSystem.models;

namespace Domain.StoreSystem.repository;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store?> AddAsync(Store store);
    Task<Store> UpdateAsync(Store store);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Store>?> GetAllAsync();
    Task<Store?> GetByEnterpriseIdAsync(Guid enterpriseId);
    
    
}