namespace BackgroundFunctions
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
        public static async Task Run([TimerTrigger("0 0 12 * * ?")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation("PeriodicalTransactionsFunction start");
                var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerContext>();
                var options = optionsBuilder
                    .UseSqlServer(EnvironmentVariableExtension.GetDbConnectionString())
                    .Options;

                await using var context = new MoneyManagerContext(options);

                var periodicalTransactions = await context.Transactions.Where(x =>
                    x.IsRepeatable == true && x.TransactionDate == DateTime.Today.AddMonths(-1)).ToListAsync();

                IList<Transaction> addedTransactions = new List<Transaction>();

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
                        UserId = transaction.UserId
                    };

                    transaction.ChildTransaction = newTransaction;

                    addedTransactions.Add(newTransaction);
                    log.LogInformation($"Finish work with transaction with Id: {transaction.TransactionId}");
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
