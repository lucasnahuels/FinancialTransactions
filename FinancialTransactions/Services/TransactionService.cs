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

        public IEnumerable<Transaction> GetTransactions() => _transactionRepository.GetTransactions();

        public async Task SaveTransactions(IEnumerable<Transaction> transactions) => await _transactionRepository.SaveTransactions(transactions);
    }
}
