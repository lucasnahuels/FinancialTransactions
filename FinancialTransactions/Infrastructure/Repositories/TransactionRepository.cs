using FinancialTransactions.Infrastructure.Repositories.Interfaces;
using FinancialTransactions.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

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
            //_dbContext.Transactions.AddRange(transactions);
            //_dbContext.SaveChanges();
            var bulkConfig = new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true
            };

            _dbContext.Database.SetCommandTimeout(1800);
            int batches = transactions.Count() / 100000;
            for (int i = 0; i < batches; i++)
            {
                _dbContext.BulkInsert(transactions.Skip(i * 100000).Take(100000), bulkConfig);
                _dbContext.SaveChanges();
            }

            //using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-KH9LQO5\\SQLEXPRESS;Initial Catalog=FinancialTransactions;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True"))
            //{
            //    connection.Open();

            //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            //    {
            //        bulkCopy.DestinationTableName = "Transactions";

            //        var transactionsDataTable = new DataTable();
            //        transactionsDataTable.Columns.Add("TransactionId", typeof(Guid));
            //        transactionsDataTable.Columns.Add("UserId", typeof(Guid));
            //        transactionsDataTable.Columns.Add("Date", typeof(DateTime));
            //        transactionsDataTable.Columns.Add("Amount", typeof(decimal));
            //        transactionsDataTable.Columns.Add("Category", typeof(string));
            //        transactionsDataTable.Columns.Add("Description", typeof(string));
            //        transactionsDataTable.Columns.Add("Merchant", typeof(string));

            //        foreach (var transaction in transactions)
            //        {
            //            transactionsDataTable.Rows.Add(
            //                transaction.TransactionId,
            //                transaction.UserId,
            //                transaction.Date,
            //                transaction.Amount,
            //                transaction.Category,
            //                transaction.Description,
            //                transaction.Merchant);
            //        }

            //        bulkCopy.WriteToServer(transactionsDataTable);
            //    }
            //}
        }
    }
}
