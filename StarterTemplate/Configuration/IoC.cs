using System.Data.Entity;
using StarterTemplate.Core;
using StarterTemplate.Core.Data;
using StarterTemplate.Core.Data.EntityFramework;
using StructureMap;

namespace StarterTemplate.Configuration
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.For<DbContext>().Use<ApplicationDbContext>();
                x.For<IReadOnlyRepository>().Use<EntityFrameworkReadOnlyRepository>();
                x.For<IRepository>().Use<EntityFrameworkRepository>();
                x.For<CurrentUserContext>().HybridHttpOrThreadLocalScoped().Use<CurrentUserContext>();
            });

            return ObjectFactory.Container;
        }
    }
}