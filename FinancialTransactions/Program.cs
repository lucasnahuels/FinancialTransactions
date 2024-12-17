using FinancialTransactions;
using FinancialTransactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using FinancialTransactions.Helpers;

// Create a service provider
var serviceProvider = IoCContainer.CreateServiceProvider();

// Create services
var transactionService = serviceProvider.GetRequiredService<ITransactionService>();
var analysisService = serviceProvider.GetRequiredService<IAnalysisService>();

// Load transactions from CSV file
var transactions = CsvHelper.LoadTransactionsFromCsv("E:\\Users\\Lucas\\Downloads\\Transactions\\transactions_2_million.csv");

// Save transactions to database
transactionService.SaveTransactions(transactions);

// Perform analysis
var userSummaries = analysisService.GetUserSummaries(transactions);
var topCategories = analysisService.GetTopCategories(transactions);
var highestSpender = analysisService.GetHighestSpender(transactions);

// Generate JSON report
var report = new
{
    users_summary = userSummaries,
    top_categories = topCategories,
    highest_spender = highestSpender
};
var jsonReport = JsonConvert.SerializeObject(report, Formatting.Indented);

// Save JSON report to file
File.WriteAllText("E:/report.json", jsonReport);

Console.WriteLine("Report generated successfully!");

