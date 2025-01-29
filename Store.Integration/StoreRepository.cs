
using CashDesk.Integration;
using Domain.StoreSystem.repository;
using Microsoft.EntityFrameworkCore;


namespace Store.Integration;

public class StoreRepository : IStoreRepository
{
    private readonly StoreContext _context;

    public StoreRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<Domain.StoreSystem.models.Store?> GetByIdAsync(Guid id)
    {
        return await _context.Stores.FindAsync(id);
    }

    public async Task<Domain.StoreSystem.models.Store?> AddAsync(Domain.StoreSystem.models.Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<Domain.StoreSystem.models.Store> UpdateAsync(Domain.StoreSystem.models.Store store)
    {
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task DeleteAsync(Guid id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store != null)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Domain.StoreSystem.models.Store>?> GetAllAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<Domain.StoreSystem.models.Store?> GetByEnterpriseIdAsync(Guid enterpriseId)
    {
        return await _context.Stores.FirstOrDefaultAsync(store => store.EnterpriseId == enterpriseId);
    }
}