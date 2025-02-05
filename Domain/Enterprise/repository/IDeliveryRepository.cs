using Domain.Enterprise.models;
using Domain.Enterprise.ValueObjects;

namespace Domain.Enterprise.repository;

public interface IDeliveryRepository
{
    public Task<List<SupplierDeliveryTime>> GetAverageSupplierDeliveryTimesByEnterpriseId(Guid enterpriseId);
}