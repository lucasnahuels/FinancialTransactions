using FinancialTransactions.Models;

namespace FinancialTransactions.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();
        void SaveTransactions(IEnumerable<Transaction> transactions);
    }
}
