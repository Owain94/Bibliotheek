#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace Bibliotheek
{
    public class RouteConfig
    {
        #region Public Methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

        #endregion Public Methods
    }
}