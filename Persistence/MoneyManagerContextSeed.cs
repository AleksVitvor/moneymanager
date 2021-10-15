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
        }
    }
}
