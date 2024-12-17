using FinancialTransactions.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactions.Repositories
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-KH9LQO5\\SQLEXPRESS;Initial Catalog=FinancialTransactions;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True");
        //}
    }
}
