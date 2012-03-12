using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting;

namespace StarterTemplate.Configuration
{
    public class Routing
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapAttributeRoutes(config=>
            {
                config.ScanAssemblyOf<MvcApplication>();
                config.UseLowercaseRoutes = true;
                config.AppendTrailingSlash = true;
            });

            routes.MapRoute("CatchAll", "{*path}", new { controller = "Error", action = "FileNotFound" });
        }        
    }
}