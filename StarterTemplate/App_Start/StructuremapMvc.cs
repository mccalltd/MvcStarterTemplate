using System.Web.Mvc;
using StarterTemplate.Configuration;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(StarterTemplate.App_Start.StructuremapMvc), "Start")]

namespace StarterTemplate.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = (IContainer) IoC.Initialize();
            DependencyResolver.SetResolver(new StructuremapDependencyResolver(container));
        }
    }
}