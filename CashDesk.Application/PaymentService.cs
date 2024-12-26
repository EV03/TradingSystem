using Domain.CashDesk;

namespace CashDesk.Application;

public class PaymentService : IPaymentService
{
    IBankService _bankService; 
    
    public PaymentService(IBankService bankService)
    {
        _bankService = bankService;
    }
    public Task<bool> PayByCash(int amount)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PayByCard(int token)
    {
        throw new NotImplementedException();
    }
}