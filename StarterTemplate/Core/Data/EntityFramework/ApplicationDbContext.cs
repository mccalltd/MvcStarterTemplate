using System.Data.Entity;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Migrations;

namespace StarterTemplate.Core.Data.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, MigrationsConfiguration>());
        }

        public DbSet<Member> Members { get; set; }
    }
}