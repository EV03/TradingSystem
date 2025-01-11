using Domain.CashDesk;

namespace CashDesk.Infrastructure.Bank;

public class SilaBankServiceAdapter : IBankService
{
    IBankServer _bankServer;
    
    public SilaBankServiceAdapter(IBankServer bankServer) 
    {
        _bankServer = bankServer;
    }
    public  Task<OperationResult<BankTransactionContext>> CreateTransactionContextAsync(int amount)
    {
        
        try
        {
            var transactionContext =  _bankServer.CreateContext(amount);

            byte[] challengeBytes;
            using (var memoryStream = new MemoryStream())
            {
                transactionContext.Challenge.CopyTo(memoryStream);
                challengeBytes = memoryStream.ToArray();
            }
            
            var bankTransactionContext = new BankTransactionContext(
                transactionContext.ContextId,
                challengeBytes,
                transactionContext.Amount
            );
            return Task.FromResult(OperationResult<BankTransactionContext>.Success(bankTransactionContext));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return Task.FromResult(OperationResult<BankTransactionContext>.Failure(ex.Message));
        }
    } 
    
    public  Task<OperationResult<AuthorizationResult>> AuthorizePaymentAsync(string contextId, string account,  string token)
    {
        try
        {   
            _bankServer.AuthorizePayment(contextId, account, token);
            var authorizationResult = new AuthorizationResult(success: true, null);
            return Task.FromResult(OperationResult<AuthorizationResult>.Success(authorizationResult));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            var authorizationResult = new AuthorizationResult(success: false, ex.Message);
            return Task.FromResult(OperationResult<AuthorizationResult>.Failure(ex.Message));
        }
        
    }
    
}