namespace Domain.CashDesk;

public interface IPaymentService
{
    Task PayCashAsync(int amount);
    
    Task PayCardAsync(int amount);
}