namespace Domain.CashDesk;

public interface IBankService
{
    Task<OperationResult<BankTransactionContext>> CreateTransactionContextAsync (int amount);
    
    Task<OperationResult<AuthorizationResult>> AuthorizePaymentAsync(string contextId, string account,  string token);
    
}