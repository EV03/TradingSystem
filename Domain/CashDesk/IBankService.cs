namespace Domain.CashDesk;

public interface IBankService
{
    Task<BankTransactionContext> CreateTransactionContextAsync(int amount);
    
    Task<AuthorizationResult> AuthorizePaymentAsync(BankTransactionContext context, string token);
    
}