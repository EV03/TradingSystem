namespace Domain.StoreSystem.repository;

public interface IStoreRepository
{
    Store GetById(Guid id);
    void Save(Store store);
    void Update(Store store);
    void Delete(Guid id);
    IEnumerable<Store> GetAll();
    Store GetByEnterpriseId(Guid enterpriseId);
    
    
}