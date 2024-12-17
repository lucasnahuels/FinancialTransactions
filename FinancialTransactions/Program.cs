using FinancialTransactions;
using FinancialTransactions.Models;
using FinancialTransactions.Repositories;
using FinancialTransactions.Services;
using FinancialTransactions.Services.Interfaces;
using FinancialTransactions.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

  
// Create a service provider
var serviceProvider = IoCContainer.CreateServiceProvider();

// Create services
//var dbContext = serviceProvider.GetService<TransactionContext>();
var transactionRepository = serviceProvider.GetService<ITransactionRepository>();
var transactionService = serviceProvider.GetService<ITransactionService>();
var analysisService = serviceProvider.GetService<IAnalysisService>();

// Load transactions from CSV file
var transactions = LoadTransactionsFromCsv("E:\\Users\\Lucas\\Downloads\\Transactions\\transactions_10_thousand.csv");

// Save transactions to database
transactionService.SaveTransactions(transactions);

// Perform analysis
var userSummaries = analysisService.GetUserSummaries();
var topCategories = analysisService.GetTopCategories();
var highestSpender = analysisService.GetHighestSpender();

// Generate JSON report
var report = new
{
    users_summary = userSummaries,
    top_categories = topCategories,
    highest_spender = highestSpender
};
var jsonReport = JsonConvert.SerializeObject(report, Formatting.Indented);

// Save JSON report to file
File.WriteAllText("report.json", jsonReport);

Console.WriteLine("Report generated successfully!");

static IEnumerable<Transaction> LoadTransactionsFromCsv(string filePath)
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
