namespace Shared.Contracts.Events;

// product transport between stores
public class  ProductTransferEvent : IEnterpriseEvent
{
    public long EnterpriseId { get; set; }
    public long ToStoreId { get; set; }
    public  List<ProductTransferDetail> ProductTransferDetails { get; set; }
}

public class ProductTransferDetail
{
    public long ProductId { get; set; }
    public long FromStoreId { get; set; }
    public int Quantity { get; set; }
}