namespace BackgroundFunctions
{
    using Domain;
    using ExchangeRateLibrary.Service;
    using Extension;
    using Microsoft.Azure.WebJobs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ExchangeRateFunction
    {
        [FunctionName("ExchangeRateFunction")]
        public static async Task Run([TimerTrigger("0 0 6 * * ?")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation("ExchangeRateFunction start");

                IExchangeRateService exchangeRateService = new ExchangeRateService();
                var todayRate = exchangeRateService.GetExchangeRate(DateTime.Today).Result;

                log.LogInformation($"Receive today rate successfully: {todayRate.Success}");

                if (todayRate.Success)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerContext>();
                    var options = optionsBuilder
                        .UseSqlServer(EnvironmentVariableExtension.GetDbConnectionString())
                        .Options;

                    IList<Currency> addedCurrencies = new List<Currency>();

                    await using var context = new MoneyManagerContext(options);

                    foreach (var rate in todayRate.Rates)
                    {
                        if (!await context.Currencies.AnyAsync(x => x.CurrencyCode == rate.Code))
                        {
                            log.LogInformation($"Start work with new currency: {rate.Code}");
                            var currency = new Currency
                            {
                                CurrencyCode = rate.Code
                            };
                            addedCurrencies.Add(currency);

                            context.ExchangeRates.Add(new ExchangeRates
                            {
                                Currency = currency,
                                Date = todayRate.Date,
                                ExchangeRate = rate.Rate
                            });
                            log.LogInformation($"Finish work with new currency: {rate.Code}");
                        }
                        else
                        {
                            log.LogInformation($"Start work with existing currency: {rate.Code}");
                            var currencyId =
                                (await context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == rate.Code))
                                .CurrencyId;

                            context.ExchangeRates.Add(new ExchangeRates
                            {
                                CurrencyId = currencyId,
                                Date = todayRate.Date,
                                ExchangeRate = rate.Rate
                            });
                            log.LogInformation($"Finish work with existing currency: {rate.Code}");
                        }
                    }

                    context.Currencies.AddRange(addedCurrencies);

                    await context.SaveChangesAsync();

                    log.LogInformation("ExchangeRateFunction finish");
                }
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
            }
        }
    }
}
