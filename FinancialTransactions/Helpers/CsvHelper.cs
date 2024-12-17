using FinancialTransactions.Models;

namespace FinancialTransactions.Helpers
{
    public static class CsvHelper
    {

        public static IEnumerable<Transaction> LoadTransactionsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skip header row
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    yield return new Transaction
                    {
                        TransactionId = Guid.Parse(values[0]),
                        UserId = Guid.Parse(values[1]),
                        Date = DateTime.Parse(values[2]),
                        Amount = decimal.Parse(values[3]),
                        Category = values[4],
                        Description = values[5],
                        Merchant = values[6]
                    };
                }
            }
        }
    }
}
