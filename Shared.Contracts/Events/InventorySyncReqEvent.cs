namespace Shared.Contracts.Events;
// enterprise requests stores to deliver they stocks
public class InventorySyncReqEvent : IEnterpriseEvent
{
    public required List<long>  ProductIds { get; set; }
    public required List<long> ExcludedStoreIds { get; set; }
    public long ToStoreId { get; set; }
    public long EnterpriseId { get; set; } }