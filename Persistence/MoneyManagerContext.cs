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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction>()
                        .HasOne(x => x.ChildTransaction)
                        .WithOne(x => x.ParentTransaction)
                        .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
