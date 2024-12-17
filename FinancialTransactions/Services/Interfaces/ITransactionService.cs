using FinancialTransactions.Models;

namespace FinancialTransactions.Services.Interfaces
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactions();
        void SaveTransactions(IEnumerable<Transaction> transactions);
    }
}
