namespace Domain.CashDesk;

public interface IBankService
{
    Task<BankTransactionContext> CreateTransactionContextAsync(double amount);
    
    Task<AuthorizationResult> AuthorizePaymentAsync(BankTransactionContext context, string token);
    
}