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
        }
    }
}
