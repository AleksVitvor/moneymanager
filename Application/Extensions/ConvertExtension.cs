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
            DateTime dateOfTransaction, 
            MoneyManagerContext context)
        {
            var exchangeRate = context.ExchangeRates
                .Include(x => x.Currency)
                .FirstOrDefault(x => x.Currency.CurrencyCode == currencyCode && x.Date == dateOfTransaction)?.ExchangeRate;

            return (float)(exchangeRate > 0 && exchangeRate.HasValue ? exchangeRate.Value : 1) * value;
        }
    }
}
