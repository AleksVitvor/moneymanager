namespace WorkerFunctions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Extension;
    using Microsoft.Azure.WebJobs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public static class PeriodicalTransactionsFunction
    {
        [FunctionName("PeriodicalTransactionsFunction")]
        public static async Task Run([TimerTrigger("0 0 4 * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation("PeriodicalTransactionsFunction start");
                var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerContext>();
                var options = optionsBuilder
                    .UseSqlServer(EnvironmentVariableExtension.GetDbConnectionString())
                    .Options;

                await using var context = new MoneyManagerContext(options);
                
                var periods = await context.TransactionPeriods
                    .ToListAsync();

                IList<Transaction> addedTransactions = new List<Transaction>();

                foreach (var period in periods)
                {
                    var periodicalTransactions = new List<Transaction>();
                    if (period.Description == "Once a week")
                    {
                        periodicalTransactions = await context.Transactions.Where(x =>
                            x.IsRepeatable && x.TransactionDate == DateTime.Today.AddDays(-7)).ToListAsync();

                        foreach (var transaction in periodicalTransactions)
                        {
                            log.LogInformation($"Start work with transaction with Id: {transaction.TransactionId}");
                            var newTransaction = new Transaction
                            {
                                Amount = transaction.Amount,
                                ParentTransactionId = transaction.TransactionId,
                                CurrencyId = transaction.CurrencyId,
                                IsRepeatable = transaction.IsRepeatable,
                                TransactionDate = transaction.TransactionDate.AddDays(7),
                                TransactionCategoryId = transaction.TransactionCategoryId,
                                TransactionTypeId = transaction.TransactionTypeId,
                                UserId = transaction.UserId,
                                TransactionPeriodId = transaction.TransactionPeriodId
                            };

                            transaction.ChildTransaction = newTransaction;

                            addedTransactions.Add(newTransaction);
                            log.LogInformation($"Finish work with transaction with Id: {transaction.TransactionId}");
                        }
                    }

                    if (period.Description == "Once a month")
                    {
                        periodicalTransactions = await context.Transactions.Where(x =>
                            x.IsRepeatable && x.TransactionDate == DateTime.Today.AddMonths(-1)).ToListAsync();

                        foreach (var transaction in periodicalTransactions)
                        {
                            log.LogInformation($"Start work with transaction with Id: {transaction.TransactionId}");
                            var newTransaction = new Transaction
                            {
                                Amount = transaction.Amount,
                                ParentTransactionId = transaction.TransactionId,
                                CurrencyId = transaction.CurrencyId,
                                IsRepeatable = transaction.IsRepeatable,
                                TransactionDate = transaction.TransactionDate.AddMonths(1),
                                TransactionCategoryId = transaction.TransactionCategoryId,
                                TransactionTypeId = transaction.TransactionTypeId,
                                UserId = transaction.UserId,
                                TransactionPeriodId = transaction.TransactionPeriodId
                            };

                            transaction.ChildTransaction = newTransaction;

                            addedTransactions.Add(newTransaction);
                            log.LogInformation($"Finish work with transaction with Id: {transaction.TransactionId}");
                        }
                    }

                    if (period.Description == "Once every three month")
                    {
                        periodicalTransactions = await context.Transactions.Where(x =>
                            x.IsRepeatable && x.TransactionDate == DateTime.Today.AddMonths(-3)).ToListAsync();

                        foreach (var transaction in periodicalTransactions)
                        {
                            log.LogInformation($"Start work with transaction with Id: {transaction.TransactionId}");
                            var newTransaction = new Transaction
                            {
                                Amount = transaction.Amount,
                                ParentTransactionId = transaction.TransactionId,
                                CurrencyId = transaction.CurrencyId,
                                IsRepeatable = transaction.IsRepeatable,
                                TransactionDate = transaction.TransactionDate.AddMonths(3),
                                TransactionCategoryId = transaction.TransactionCategoryId,
                                TransactionTypeId = transaction.TransactionTypeId,
                                UserId = transaction.UserId,
                                TransactionPeriodId = transaction.TransactionPeriodId
                            };

                            transaction.ChildTransaction = newTransaction;

                            addedTransactions.Add(newTransaction);
                            log.LogInformation($"Finish work with transaction with Id: {transaction.TransactionId}");
                        }
                    }

                    if (period.Description == "Once a year")
                    {
                        periodicalTransactions = await context.Transactions.Where(x =>
                            x.IsRepeatable && x.TransactionDate == DateTime.Today.AddYears(-1)).ToListAsync();

                        foreach (var transaction in periodicalTransactions)
                        {
                            log.LogInformation($"Start work with transaction with Id: {transaction.TransactionId}");
                            var newTransaction = new Transaction
                            {
                                Amount = transaction.Amount,
                                ParentTransactionId = transaction.TransactionId,
                                CurrencyId = transaction.CurrencyId,
                                IsRepeatable = transaction.IsRepeatable,
                                TransactionDate = transaction.TransactionDate.AddYears(1),
                                TransactionCategoryId = transaction.TransactionCategoryId,
                                TransactionTypeId = transaction.TransactionTypeId,
                                UserId = transaction.UserId,
                                TransactionPeriodId = transaction.TransactionPeriodId
                            };

                            transaction.ChildTransaction = newTransaction;

                            addedTransactions.Add(newTransaction);
                            log.LogInformation($"Finish work with transaction with Id: {transaction.TransactionId}");
                        }
                    }
                }

                await context.Transactions.AddRangeAsync(addedTransactions);

                await context.SaveChangesAsync();
                log.LogInformation("PeriodicalTransactionsFunction end");
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
            }
        }
    }
}
