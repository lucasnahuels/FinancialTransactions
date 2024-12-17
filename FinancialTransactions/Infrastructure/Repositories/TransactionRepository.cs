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
            var tableName = "Transactions";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                DisableConstraints(connection, tableName);
                using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, sqlTransaction))
                        {
                            bulkCopy.DestinationTableName = tableName;

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
                EnableConstraints(connection, tableName);
            }
        }

        private static void EnableConstraints(SqlConnection connection, string tableName)
        {
            using (SqlCommand command = new SqlCommand("ALTER TABLE " + tableName + " CHECK CONSTRAINT ALL", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void DisableConstraints(SqlConnection connection, string tableName)
        {
            using (SqlCommand command = new SqlCommand("ALTER TABLE " + tableName + " NOCHECK CONSTRAINT ALL", connection))
            {
                command.ExecuteNonQuery();
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
