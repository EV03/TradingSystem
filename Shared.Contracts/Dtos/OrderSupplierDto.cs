using Domain.StoreSystem.models;

namespace Shared.Contracts.Dtos;

public class OrderSupplierDto
{
    public long Id { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public List<OrderSupplierCachedProductDto> OrderSupplierProducts { get; set; }
}