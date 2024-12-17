namespace FinancialTransactions.Models
{
    public class HighestSpender
    {
        public Guid UserId { get; set; }
        public decimal TotalExpense { get; set; }
    }
}
