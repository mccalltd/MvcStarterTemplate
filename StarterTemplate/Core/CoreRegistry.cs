using StarterTemplate.Core.Data;
using StarterTemplate.Core.Data.EntityFramework;
using StructureMap.Configuration.DSL;

namespace StarterTemplate.Core
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            Scan(scan =>
            {
                scan.AssemblyContainingType<CoreRegistry>();
                scan.WithDefaultConventions();
            });

            For<IRepository>().Use<EntityFrameworkRepository>();
        }
    }
}
