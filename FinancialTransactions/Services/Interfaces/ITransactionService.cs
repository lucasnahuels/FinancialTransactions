using FinancialTransactions.Models;

namespace FinancialTransactions.Services.Interfaces
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactions();
        Task SaveTransactions(IEnumerable<Transaction> transactions);
    }
}
