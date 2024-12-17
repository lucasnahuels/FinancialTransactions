using FinancialTransactions.Models;

namespace FinancialTransactions.Services.Interfaces
{
    public interface IAnalysisService
    {
        UserSummary[] GetUserSummaries(IEnumerable<Transaction> transactions);
        TopCategory[] GetTopCategories(IEnumerable<Transaction> transactions);
        HighestSpender GetHighestSpender(IEnumerable<Transaction> transactions);
    }

}
