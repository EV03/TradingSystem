namespace Domain.StoreSystem.repository;

public interface IStoreRepository
{
    Task<Store> GetById(Guid id);
    Task<Store> Save(Store store);
    Task<Store> Update(Store store);
    Task Delete(Guid id);
    IEnumerable<Store> GetAll();
    Store GetByEnterpriseId(Guid enterpriseId);
    
    
}