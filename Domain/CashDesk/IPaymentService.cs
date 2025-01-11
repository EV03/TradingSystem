namespace Domain.CashDesk;

public interface IPaymentService
{
    Task<bool> PayCash(int amount);
    
    Task<bool> PayCard(int amount);
}