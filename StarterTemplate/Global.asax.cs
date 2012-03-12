using System.Web.Mvc;
using System.Web.Routing;
using StarterTemplate.Configuration;

namespace StarterTemplate
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            Routing.RegisterRoutes(RouteTable.Routes);
            IoC.Initialize();

            ModelMetadataProviders.Current = new ApplicationModelMetadataProvider();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}