namespace Shared.Contracts.Events;
// enterprise requests stores to deliver they stocks
public class InventorySyncEvent
{
    public long ProductId { get; set; }
    public List<String> ExcludedStores { get; set; }
}