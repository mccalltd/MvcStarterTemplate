using System;
using System.Data.Entity.Migrations;
using DevOne.Security.Cryptography.BCrypt;
using StarterTemplate.Core.Data.EntityFramework;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Migrations
{
    public sealed class MigrationsConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var now = DateTime.Now;

            context.Members.AddOrUpdate(
                p => p.EmailAddress,
                new Member
                {
                    EmailAddress = "webmaster@domain.com",
                    PasswordHash = BCryptHelper.HashPassword("webmaster", BCryptHelper.GenerateSalt(12)),
                    IsAdministrator = true,
                    CreatedByMemberId = -1,
                    CreatedDate = now,
                    ModifiedByMemberId = -1,
                    ModifiedDate = now
                });
        }
    }
}
