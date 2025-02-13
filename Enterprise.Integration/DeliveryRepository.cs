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
    public async Task<List<SupplierDeliveryTime>> GetAverageSupplierDeliveryTimesBy()
    {
        return await _context.DeliveryLogs
            .GroupBy(log => log.SupplierId)
            .Select(group => new SupplierDeliveryTime()
            {
                SupplierId = group.First().SupplierId,
                AverageDeliveryTime = (int)group.Average(log => (log.DeliveryDate - log.OrderDate).TotalDays)
            })
            .ToListAsync();
    }
}