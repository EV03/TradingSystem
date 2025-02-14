using Domain.Enterprise.models;
using Domain.Enterprise.repository;
using Domain.Enterprise.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Integration;

public class DeliveryLogLogRepository : IDeliveryLogRepository
{
    private readonly EnterpriseContext _context;

    public DeliveryLogLogRepository(EnterpriseContext context)
    {
        _context = context;
    }
    public async Task<List<SupplierDeliveryTime>> GetAverageSupplierDeliveryTimes()
    {
        return await _context.DeliveryLogs
            .Where(log => log.DeliveryDate != null)
            .GroupBy(log => log.SupplierId)
            .Select(group => new SupplierDeliveryTime
            {
                SupplierId = group.Key,
                AverageDeliveryTime = GetAverageDeliveryTime(group.Average(log => 
                    (log.DeliveryDate!.Value - log.OrderDate).TotalSeconds))
            })
            .ToListAsync();
    }
    
    private string GetAverageDeliveryTime(double averageSeconds)
    {
        if (averageSeconds >= 86400) 
        {
            return $"{averageSeconds / 86400:F2} days";
        } 
        if (averageSeconds >= 3600) 
        {
            return $"{averageSeconds / 3600:F2} hours";
        } 
        if (averageSeconds >= 60) 
        { 
            return $"{averageSeconds / 60:F2} minutes";
        }
        {
            return $"{averageSeconds:F2} seconds";
        }
    }
    
    public async Task<DeliveryLog> AddDeliveryLogAsync(DeliveryLog deliveryLog)
    {
        await _context.DeliveryLogs.AddAsync(deliveryLog);
        await _context.SaveChangesAsync();
        return deliveryLog;
    }

    public Task<DeliveryLog?> GetDeliveryLogByOrderSupplierIdAsync(long id)
    {
        return _context.DeliveryLogs
            .Where(log => log.OrderSupplierId == id)
            .FirstOrDefaultAsync();
    }

    public Task UpdateAsync(DeliveryLog deliveryLog)
    {
        _context.DeliveryLogs.Update(deliveryLog);
        return _context.SaveChangesAsync();
    }
}