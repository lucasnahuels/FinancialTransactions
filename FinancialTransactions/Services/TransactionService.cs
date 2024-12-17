using FinancialTransactions.Infrastructure.Repositories.Interfaces;
using FinancialTransactions.Models;
using FinancialTransactions.Services.Interfaces;

namespace FinancialTransactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return _transactionRepository.GetTransactions();
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            _transactionRepository.SaveTransactions(transactions);
        }
    }
}
