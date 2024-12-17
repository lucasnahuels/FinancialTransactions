using FinancialTransactions.Models;

namespace FinancialTransactions.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();
        void SaveTransactions(IEnumerable<Transaction> transactions);
    }
}
