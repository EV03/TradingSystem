namespace Domain.CashDesk;

public interface IPaymentService
{
    Task<bool> PayCashAsync(int amount);
    
    Task<bool> PayCardAsync(int amount);
}