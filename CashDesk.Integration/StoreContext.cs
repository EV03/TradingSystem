namespace CashDesk.Integration;

public class StoreContext
{
    public string StoreId { get; private set; }
    public string CashDeskId { get; private set; }

    public StoreContext(string storeId, string cashDeskId)
    {
        StoreId = storeId;
        CashDeskId = cashDeskId;
    }
}