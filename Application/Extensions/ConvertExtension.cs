namespace Application.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Linq;

    public static class ConvertExtension
    {
        public static float ConvertToCurrency(
            this float value, 
            string currencyCode,
            string transactionCurrencyCode,
            DateTime dateOfTransaction, 
            MoneyManagerContext context)
        {
            var transactionToEur = context.ExchangeRates
                .Include(x => x.Currency)
                .FirstOrDefault(x => x.Currency.CurrencyCode == transactionCurrencyCode && x.Date == dateOfTransaction)?.ExchangeRate;

            var convertedValue = value / (float)(transactionToEur is > 0 ? transactionToEur.Value : 1);

            var exchangeRate = context.ExchangeRates
                .Include(x => x.Currency)
                .FirstOrDefault(x => x.Currency.CurrencyCode == currencyCode && x.Date == dateOfTransaction)?.ExchangeRate;

            return (float)(exchangeRate is > 0 ? exchangeRate.Value : 1) * convertedValue;
        }
    }
}
