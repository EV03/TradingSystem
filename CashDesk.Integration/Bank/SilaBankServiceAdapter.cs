using Grpc.Core; // Für RpcException
using Shared.Contracts.Interfaces;

namespace CashDesk.Integration.Bank;

public class SilaBankServiceAdapter : IBankService
{
    private readonly IBankServer _bankServer;
    
    public SilaBankServiceAdapter(IBankServer bankServer) 
    {
        _bankServer = bankServer;
    }
    
    public async Task<IBankService.BankTransactionContext> CreateTransactionContextAsync(long amount)
    {
        try
        {
            var transactionContext = _bankServer.CreateContext(amount);
            
            byte[] challengeBytes;
            using (var memoryStream = new MemoryStream())
            {
                transactionContext.Challenge.CopyTo(memoryStream);
                challengeBytes = memoryStream.ToArray();
            }
            
            return new IBankService.BankTransactionContext(
                transactionContext.ContextId,
                challengeBytes,
                transactionContext.Amount
            );
        }
        catch (RpcException rpcEx)
        {
            HandleRpcException(rpcEx);
            throw; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw; 
        }
    }

    public async Task AuthorizePaymentAsync(string contextId, string account, string token)
    {
        try
        {
            _bankServer.AuthorizePayment(contextId, account, token);
        }
        catch (RpcException rpcEx)
        {
            HandleRpcException(rpcEx);
            throw; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw; 
        }
    }

    private void HandleRpcException(RpcException rpcEx)
    {
        switch (rpcEx.Status.StatusCode)
        {
            case StatusCode.InvalidArgument:
            case StatusCode.NotFound:
                // no retry 
                Console.WriteLine($"Logical error: {rpcEx.Status.StatusCode} - {rpcEx.Status.Detail}");
                throw rpcEx;

            case StatusCode.Unavailable:
            case StatusCode.DeadlineExceeded:
                // netwerk error retry could be used 
                Console.WriteLine($"Network error: {rpcEx.Status.StatusCode} - Retrying might be possible.");
                throw rpcEx;

            default:
                Console.WriteLine($"Unhandled gRPC error: {rpcEx.Status.StatusCode} - {rpcEx.Status.Detail}");
                throw rpcEx; 
        }
    }
}
