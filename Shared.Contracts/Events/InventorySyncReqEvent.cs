namespace Shared.Contracts.Events;
// enterprise requests stores to deliver they stocks
public class InventorySyncReqEvent : IEnterpriseEvent
{
    public  List<long>  ProductIds { get; set; }  = new List<long>();
    public  List<long> ExcludedStoreIds { get; set; } = new List<long>();
    public long ToStoreId { get; set; }
    public long EnterpriseId { get; set; } }