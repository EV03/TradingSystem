namespace Shared.Contracts.Interfaces;

public interface IBankService
{
    Task<BankTransactionContext> CreateTransactionContextAsync (int amount);
    
    Task AuthorizePaymentAsync(string contextId, string account,  string token);
    
    public class BankTransactionContext
    {
        public string Id { get; }
        public byte[] Challenge { get; }
        public long Amount { get; }

        public BankTransactionContext(string id, byte[] challenge, long amount)
        {
            Id = id;
            Challenge = challenge;
            Amount = amount;
        }
    }
    
}