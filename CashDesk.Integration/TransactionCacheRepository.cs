
using System.Text.Json;
using Domain.CashDesk;

namespace CashDesk.Integration
{
    public class TransactionCacheRepository : ITransactionRepository
    {
        private readonly string _filePath = "transactions.json";
        private readonly List<Transaction> _transactions;
        private readonly object _lock = new object();

        public TransactionCacheRepository()
        {
            _transactions = LoadTransactionsFromFile();
        }

        public void SaveTransaction(Transaction transaction)
        {
            lock (_lock)
            {
                _transactions.Add(transaction);
                SaveTransactionsToFile();
            }
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            lock (_lock)
            {
                return new List<Transaction>(_transactions);
            }
        }

        public IEnumerable<Transaction> GetRecentTransactions()
        {
            lock (_lock)
            {
                var cutoffTime = DateTime.UtcNow.AddMinutes(-60);
                return _transactions.Where(t => t.Timestamp >= cutoffTime).ToList();
            }
        }

        public void DeleteTransactions()
        {
            lock (_lock)
            {
                _transactions.Clear();
                SaveTransactionsToFile();
            }
        }

        private List<Transaction> LoadTransactionsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Transaction>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        private void SaveTransactionsToFile()
        {
            var json = JsonSerializer.Serialize(_transactions);
            File.WriteAllText(_filePath, json);
        }
    }
}