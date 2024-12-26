using Domain.CashDesk;

namespace CashDesk.Infrastructure.Bank;

public class SilaBankServiceAdapter : IBankService
{
    
    IBankServer _bankServer;
    TransactionContext _transactionContext;
    
    public SilaBankServiceAdapter(IBankServer bankServer) 
    {
        _bankServer = bankServer;
    }
    public Task<BankTransactionContext> CreateTransactionContextAsync(int amount)
    {
       _transactionContext = _bankServer.CreateContext(amount);
       DateTime timestamp = DateTime.Now;
       BankTransactionContext bankTransactionContext = new BankTransactionContext(_transactionContext.ContextId, _transactionContext.Challenge.ToString(), _transactionContext.Amount, timestamp);
       return Task.FromResult(bankTransactionContext);
    } 

    public Task<AuthorizationResult> AuthorizePaymentAsync(BankTransactionContext context, string token)
    {
        try
        {
            _bankServer.AuthorizePayment(context.Id, context.Challenge, token);
            return Task.FromResult(new AuthorizationResult(true, null));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return Task.FromResult(new AuthorizationResult(false, ex.Message));
        }
        
    }
    
}