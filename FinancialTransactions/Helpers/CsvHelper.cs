using FinancialTransactions.Models;

namespace FinancialTransactions.Helpers
{
    public static class CsvHelper
    {
        public static async Task<IEnumerable<Transaction>> LoadTransactionsFromCsv(string filePath)
        {
            var transactions = new List<Transaction>();
            using (var reader = new StreamReader(filePath))
            {
                await reader.ReadLineAsync(); // Skip header row
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');
                    transactions.Add(new Transaction
                    {
                        TransactionId = Guid.Parse(values[0]),
                        UserId = Guid.Parse(values[1]),
                        Date = DateTime.Parse(values[2]),
                        Amount = decimal.Parse(values[3]),
                        Category = values[4],
                        Description = values[5],
                        Merchant = values[6]
                    });
                }
            }
            return transactions;
        }
    }
}
