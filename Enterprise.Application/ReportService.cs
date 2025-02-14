using Domain.Enterprise.repository;
using Domain.Enterprise.ValueObjects;
using Enterprise.Application.Services;

namespace Enterprise.Application;

public class ReportService : IReportService
{
    private readonly IDeliveryLogRepository _deliveryLogRepository;
    
    public ReportService(IDeliveryLogRepository deliveryLogRepository)
    {
        _deliveryLogRepository = deliveryLogRepository;
    }

    public async Task<List<SupplierDeliveryTime>> GetSupplierDeliveryTimes()
    {
        var deliveryTimes = await _deliveryLogRepository.GetAverageSupplierDeliveryTimes();
        return deliveryTimes;
    }
}
