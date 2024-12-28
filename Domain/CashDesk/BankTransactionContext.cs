namespace Domain.CashDesk;

public class BankTransactionContext
{
    public string Id { get; set; }
    
    public byte[] Challenge { get; set; }
    
    public double Amount { get; set; }
    
    public BankTransactionContext(string id, byte[] challenge, double amount)
    {
        Id = id;
        Challenge = challenge;
        Amount = amount;
    }
    
}