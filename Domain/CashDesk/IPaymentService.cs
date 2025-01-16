namespace Domain.CashDesk;

public interface IPaymentService
{
    Task PayCashAsync(long amount);
    
    Task PayCardAsync(long amount);
}