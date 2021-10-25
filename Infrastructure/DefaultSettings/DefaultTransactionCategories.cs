namespace Infrastructure.DefaultSettings
{
    using Domain;
    using System.Collections.Generic;

    public static class DefaultTransactionCategories
    {
        public static List<TransactionCategory> TransactionCategories { get; } = new List<TransactionCategory>()
        {
            new TransactionCategory()
            {
                Description = "Auto"
            },
            new TransactionCategory()
            {
                Description = "Food"
            },
            new TransactionCategory()
            {
                Description = "Health"
            },
            new TransactionCategory()
            {
                Description = "Housing"
            },
            new TransactionCategory()
            {
                Description = "Salary"
            },
            new TransactionCategory()
            {
                Description = "Universal"
            }
        };
    }
}
