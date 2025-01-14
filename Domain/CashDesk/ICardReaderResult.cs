namespace Domain.CashDesk;

public interface ICardReaderResult
{
    string Account { get; }
    string Token { get; }
}