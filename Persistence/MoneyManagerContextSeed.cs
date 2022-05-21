namespace Persistence
{
    using Domain;
    using System.Collections.Generic;
    using System.Linq;

    public static class MoneyManagerContextSeed
    {
        public static void SeedSampleData(MoneyManagerContext context)
        {
            IList<Role> sampleRole = new List<Role>();

            IList<TransactionType> transactionTypes = new List<TransactionType>();

            IList<Currency> currencies = new List<Currency>();

            IList<TransactionPeriod> transactionPeriods = new List<TransactionPeriod>();

            if (!context.Roles.Any())
            {
                sampleRole.Add(new Role()
                {
                    Description = "User"
                });
                sampleRole.Add(new Role()
                {
                    Description = "Admin"
                });

                context.Roles.AddRange(sampleRole);

                context.SaveChanges();
            }

            if (!context.TransactionTypes.Any())
            {
                transactionTypes.Add(new TransactionType()
                {
                    Description = "Refill"
                });

                transactionTypes.Add(new TransactionType()
                {
                    Description = "Expenses"
                });

                context.TransactionTypes.AddRange(transactionTypes);

                context.SaveChanges();
            }

            if (!context.Currencies.Any())
            {
                currencies.Add(new Currency
                {
                    CurrencyCode = "EUR",
                    CurrencySymbol = "€"
                });

                currencies.Add(new Currency
                {
                    CurrencyCode = "USD",
                    CurrencySymbol = "$"
                });

                context.Currencies.AddRange(currencies);

                context.SaveChanges();
            }

            if (!context.TransactionPeriods.Any())
            {
                transactionPeriods.Add(new TransactionPeriod
                {
                    Description = "Once a week"
                });

                transactionPeriods.Add(new TransactionPeriod
                {
                    Description = "Once a month"
                });

                transactionPeriods.Add(new TransactionPeriod
                {
                    Description = "Once every three month"
                });

                transactionPeriods.Add(new TransactionPeriod
                {
                    Description = "Once a year"
                });

                context.TransactionPeriods.AddRange(transactionPeriods);

                context.SaveChanges();
            }
        }
    }
}
