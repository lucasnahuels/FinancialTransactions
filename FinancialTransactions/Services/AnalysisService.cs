using FinancialTransactions.Models;
using FinancialTransactions.Services.Interfaces;

namespace FinancialTransactions.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly ITransactionService _transactionService;

        public AnalysisService(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public UserSummary[] GetUserSummaries()
        {
            var transactions = _transactionService.GetTransactions();
            var userSummaries = transactions
                .GroupBy(t => t.UserId)
                .Select(g => new UserSummary
                {
                    UserId = g.Key,
                    TotalIncome = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                    TotalExpense = g.Where(t => t.Amount < 0).Sum(t => t.Amount)
                })
                .ToArray();
            return userSummaries;
        }

        public TopCategory[] GetTopCategories()
        {
            var transactions = _transactionService.GetTransactions();
            var topCategories = transactions
                .GroupBy(t => t.Category)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => new TopCategory
                {
                    Category = g.Key,
                    TransactionsCount = g.Count()
                })
                .ToArray();
            return topCategories;
        }

        public HighestSpender GetHighestSpender()
        {
            var transactions = _transactionService.GetTransactions();
            //var highestSpender = transactions
            //    .Where(t => t.Amount < 0)
            //    .GroupBy(t => t.UserId)
            //    .OrderByDescending(g => g.Sum(t => t.Amount))
            //    .FirstOrDefault()
            //    ?.Select(g => new HighestSpender
            //    {
            //        UserId = g.Key,
            //        TotalExpense = g.Sum(t => t.Amount)
            //    })
            //    .FirstOrDefault();

            var highestSpender = transactions
            .Where(t => t.Amount < 0)
            .GroupBy(t => t.UserId)
            .Select(g => new HighestSpender
            {
                UserId = g.Key,
                TotalExpense = g.Sum(t => t.Amount)
            })
            .OrderByDescending(g => g.TotalExpense)
            .FirstOrDefault();
            return highestSpender;
        }
    }
}
