using Domain.CashDesk;

namespace CashDesk.Infrastructure.Bank;

public class SilaBankServiceAdapter : IBankService
{
    public Task<BankTransactionContext> CreateTransactionContextAsync(double amount)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorizationResult> AuthorizePaymentAsync(BankTransactionContext context, string token)
    {
        throw new NotImplementedException();
    }
    
}