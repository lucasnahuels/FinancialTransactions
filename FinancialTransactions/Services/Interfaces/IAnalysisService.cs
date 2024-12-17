using FinancialTransactions.Models;

namespace FinancialTransactions.Services.Interfaces
{
    public interface IAnalysisService
    {
        UserSummary[] GetUserSummaries();
        TopCategory[] GetTopCategories();
        HighestSpender GetHighestSpender();
    }

}
