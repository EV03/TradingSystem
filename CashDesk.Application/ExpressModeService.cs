using Domain.CashDesk;

namespace CashDesk.Application;

public class ExpressModeService : IExpressModeService
{
    ITransactionRepository _transactionRepository;
    
    public ExpressModeService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public bool IsExpressMode()
    {
       var recentTransactions = _transactionRepository.GetRecentTransactions();
       int expressCheckoutCount = recentTransactions.Count(t => t.SaleItems.Count <= 8 && t.PaymentMehod == "Cash");
       int totalTransactions = recentTransactions.Count();
       
       return totalTransactions > 0 && (expressCheckoutCount / (double)totalTransactions) >= 0.5;
    }
}