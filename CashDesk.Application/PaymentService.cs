using Domain.CashDesk;

namespace CashDesk.Application;

public class PaymentService : IPaymentService
{
    IBankService _bankService; 
    ISaleService _saleService;
    
    public PaymentService(IBankService bankService, ISaleService saleService)
    {
        _bankService = bankService;
        _saleService = saleService;
    }
    public Task<bool> PayCash(int amount)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PayCard(int amount)
    {
        throw new NotImplementedException();
    }
}