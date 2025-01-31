using FinancialTransactions;
using FinancialTransactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using FinancialTransactions.Helpers;
using System.Diagnostics;

// Create a service provider
var serviceProvider = IoCContainer.CreateServiceProvider();

// Create services
var transactionService = serviceProvider.GetRequiredService<ITransactionService>();
var analysisService = serviceProvider.GetRequiredService<IAnalysisService>();

var stopwatch = Stopwatch.StartNew();

// Load transactions from CSV file
var transactions = CsvHelper.LoadTransactionsFromCsv("your_path_to_transactions_2_million.csv");
Console.WriteLine("CSV data read");

// Save transactions to database
await transactionService.SaveTransactions(transactions);
Console.WriteLine("\nTransactions saved in the Database");

// Perform analysis
//change this code to perform this calculations in database instead of doing it in memory  
var userSummariesTask = Task.Run(() => analysisService.GetUserSummaries(transactions));
var topCategoriesTask = Task.Run(() => analysisService.GetTopCategories(transactions));
var highestSpenderTask = Task.Run(() => analysisService.GetHighestSpender(transactions));

await Task.WhenAll(userSummariesTask, topCategoriesTask, highestSpenderTask);

var userSummaries = userSummariesTask.Result;
var topCategories = topCategoriesTask.Result;
var highestSpender = highestSpenderTask.Result;

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

Console.WriteLine("\nReport generated successfully!");

stopwatch.Stop();
Console.WriteLine($"\nProcess took {stopwatch.Elapsed.TotalSeconds} seconds");
