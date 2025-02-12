using Domain.Enterprise.models;
using Domain.Enterprise.repository;
using Domain.Enterprise.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Integration;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly EnterpriseContext _context;

    public DeliveryRepository(EnterpriseContext context)
    {
        _context = context;
    }
    public async Task<List<SupplierDeliveryTime>> GetAverageSupplierDeliveryTimesByEnterpriseId(int enterpriseId)
    {
        return await _context.DeliveryLogs
            .Where(log => log.EnterpriseId == enterpriseId)
            .GroupBy(log => log.SupplierId)
            .Select(group => new SupplierDeliveryTime()
            {
                SupplierName = group.First().SupplierName,
                AverageDeliveryTime = (int)group.Average(log => (log.DeliveryDate - log.OrderDate).TotalDays)
            })
            .ToListAsync();
    }
}