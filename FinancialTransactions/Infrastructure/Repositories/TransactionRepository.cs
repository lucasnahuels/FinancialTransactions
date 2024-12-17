using FinancialTransactions.Infrastructure.Repositories.Interfaces;
using FinancialTransactions.Models;
using FinancialTransactions.Infrastructure.Repositories;

namespace FinancialTransactions.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext _dbContext;

        public TransactionRepository(TransactionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return _dbContext.Transactions.ToList();
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            _dbContext.Transactions.AddRange(transactions);
            _dbContext.SaveChanges();
        }
    }
}
