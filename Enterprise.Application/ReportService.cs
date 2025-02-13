using Domain.Enterprise.repository;
using Domain.Enterprise.ValueObjects;
using Enterprise.Application.Services;

namespace Enterprise.Application;

public class ReportService : IReportService
{
    private readonly IDeliveryRepository _deliveryRepository;
    
    public ReportService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<List<SupplierDeliveryTime>> GetSupplierDeliveryTimes()
    {
        var deliveryTimes = await _deliveryRepository.GetAverageSupplierDeliveryTimesBy();
        return deliveryTimes;
    }
}
