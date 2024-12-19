using FinancialTransactions.Models;

namespace FinancialTransactions.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();
        Task SaveTransactions(IEnumerable<Transaction> transactions);
    }
}
