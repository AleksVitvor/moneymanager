namespace Persistence
{
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class MoneyManagerContext : DbContext
    {
        public MoneyManagerContext(DbContextOptions<MoneyManagerContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<TransactionCategory> TransactionCategories { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<ExchangeRates> ExchangeRates { get; set; }

        public DbSet<TransactionPeriod> TransactionPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction>()
                        .HasOne(x => x.ChildTransaction)
                        .WithOne(x => x.ParentTransaction)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransactionCategory>()
                .HasOne(x=>x.User)
                .WithMany(x=>x.TransactionCategories)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
