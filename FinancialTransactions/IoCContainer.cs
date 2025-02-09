﻿using FinancialTransactions.Infrastructure.Repositories;
using FinancialTransactions.Infrastructure;
using FinancialTransactions.Services.Interfaces;
using FinancialTransactions.Services;
using Microsoft.Extensions.DependencyInjection;
using FinancialTransactions.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FinancialTransactions
{
    public static class IoCContainer
    {
        public static IServiceProvider CreateServiceProvider()
        {

            //// Create a service collection
            var services = new ServiceCollection();

            // Register DbContext
            services.AddDbContext<TransactionContext>();

            // Register Config
            services.AddSingleton<IConfiguration>(provider =>
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build();
            });

            // Register repositories
            services.AddTransient<ITransactionRepository>(provider =>
            {
                var dbContext = provider.GetRequiredService<TransactionContext>();
                var config = provider.GetRequiredService<IConfiguration>();
                return new TransactionRepository(dbContext, config);
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
