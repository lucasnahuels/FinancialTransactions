using FinancialTransactions.Repositories.Interfaces;
using FinancialTransactions.Repositories;
using FinancialTransactions.Services.Interfaces;
using FinancialTransactions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactions
{
    public static class IoCContainer
    {
        public static IServiceProvider CreateServiceProvider()
        {
            // Create a configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //// Create a service collection
            var services = new ServiceCollection();

            // Register DbContext
            services.AddDbContext<TransactionContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            //services.AddTransient<ITransactionRepository, TransactionRepository>();
            //services.AddTransient<IAnalysisService, AnalysisService>();
            //services.AddTransient<ITransactionService, TransactionService>();

            services.AddTransient<ITransactionRepository>(provider =>
            {
                var dbContext = provider.GetRequiredService<TransactionContext>();
                return new TransactionRepository(dbContext);
            });

            // Register services
            services.AddTransient<ITransactionService>(provider =>
            {
                var transactionRepository = provider.GetRequiredService<ITransactionRepository>();
                return new TransactionService(transactionRepository);
            });
            services.AddTransient<IAnalysisService>(provider =>
            {
                var transactionService = provider.GetRequiredService<ITransactionService>();
                return new AnalysisService(transactionService);
            });
            
            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}
