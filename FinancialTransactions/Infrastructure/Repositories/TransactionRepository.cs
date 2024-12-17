using FinancialTransactions.Infrastructure.Repositories.Interfaces;
using FinancialTransactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace FinancialTransactions.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext _dbContext;
        private readonly IConfiguration _configuration;

        public TransactionRepository(TransactionContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return _dbContext.Transactions.ToList();
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls, sqlTransaction))
                        {
                            bulkCopy.DestinationTableName = "Transactions";

                            DataTable transactionsDataTable = GetDataTable(transactions);

                            DataTable batchDataTable = transactionsDataTable.Clone();
                            int batchSize = 200000;
                            int recordCount = 0;
                            foreach (DataRow row in transactionsDataTable.Rows)
                            {
                                batchDataTable.ImportRow(row);
                                recordCount++;

                                if (recordCount >= batchSize)
                                {
                                    bulkCopy.WriteToServer(batchDataTable);
                                    batchDataTable.Clear();
                                    recordCount = 0;
                                }
                            }

                            if (recordCount > 0)
                            {
                                bulkCopy.WriteToServer(batchDataTable);
                            }
                        }
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static DataTable GetDataTable(IEnumerable<Transaction> transactions)
        {
            var transactionsDataTable = new DataTable();
            transactionsDataTable.Columns.Add("TransactionId", typeof(Guid));
            transactionsDataTable.Columns.Add("UserId", typeof(Guid));
            transactionsDataTable.Columns.Add("Date", typeof(DateTime));
            transactionsDataTable.Columns.Add("Amount", typeof(decimal));
            transactionsDataTable.Columns.Add("Category", typeof(string));
            transactionsDataTable.Columns.Add("Description", typeof(string));
            transactionsDataTable.Columns.Add("Merchant", typeof(string));

            foreach (var transaction in transactions)
            {
                transactionsDataTable.Rows.Add(
                    transaction.TransactionId,
                    transaction.UserId,
                    transaction.Date,
                    transaction.Amount,
                    transaction.Category,
                    transaction.Description,
                    transaction.Merchant);
            }

            return transactionsDataTable;
        }
    }
}
