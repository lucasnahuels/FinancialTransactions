using FinancialTransactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialTransactions.Infrastructure
{
    public class TransactionContext : DbContext
    {
        public TransactionContext() { }
        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
