using Domain.Enterprise.ValueObjects;

namespace Enterprise.Application.Services;

public interface IReportService
{
    Task<List<SupplierDeliveryTime>> GetSupplierDeliveryTimes();
}