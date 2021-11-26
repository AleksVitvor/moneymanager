namespace WorkerFunctions
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
        public static async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer, ILogger log)
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
                    IList<ExchangeRates> newRates = new List<ExchangeRates>();

                    await using var context = new MoneyManagerContext(options);

                    var eurId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "EUR"))?.CurrencyId;
                    var usdId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "USD"))?.CurrencyId;
                    var gbrId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "GBP"))?.CurrencyId;
                    var chfId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "CHF"))?.CurrencyId;
                    var bynId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "BYN"))?.CurrencyId;
                    var rubId = (await context.Currencies.FirstOrDefaultAsync(currency => currency.CurrencyCode == "RUB"))?.CurrencyId;

                    if (!rubId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "RUB"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.RUB,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = rubId.Value,
                            ExchangeRate = todayRate.Rates.RUB,
                            Date = todayRate.Date
                        });
                    }

                    if (!bynId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "BYN"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.BYN,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = bynId.Value,
                            ExchangeRate = todayRate.Rates.BYN,
                            Date = todayRate.Date
                        });
                    }

                    if (!eurId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "EUR"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.EUR,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = eurId.Value,
                            ExchangeRate = todayRate.Rates.EUR,
                            Date = todayRate.Date
                        });
                    }

                    if (!usdId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "USD"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.USD,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = usdId.Value,
                            ExchangeRate = todayRate.Rates.USD,
                            Date = todayRate.Date
                        });
                    }

                    if (!gbrId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "GBP"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.GBP,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = gbrId.Value,
                            ExchangeRate = todayRate.Rates.GBP,
                            Date = todayRate.Date
                        });
                    }

                    if (!chfId.HasValue)
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = "CHF"
                        };
                        addedCurrencies.Add(newCurrency);
                        newRates.Add(new ExchangeRates
                        {
                            Currency = newCurrency,
                            ExchangeRate = todayRate.Rates.CHF,
                            Date = todayRate.Date
                        });
                    }
                    else
                    {
                        newRates.Add(new ExchangeRates
                        {
                            CurrencyId = chfId.Value,
                            ExchangeRate = todayRate.Rates.CHF,
                            Date = todayRate.Date
                        });
                    }

                    context.Currencies.AddRange(addedCurrencies);
                    context.ExchangeRates.AddRange(newRates);

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
