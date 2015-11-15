using System.Web.Mvc;
using System.Web.Routing;

namespace FoodRecipe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Browse", id = UrlParameter.Optional }
            );
        }
    }
}