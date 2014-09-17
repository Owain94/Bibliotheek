#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace Bibliotheek
{
    public static class RouteConfig
    {
        #region Public Methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("SingleAuthor", "Book/SingleAuthor/{name}", new { controller = "Book", action = "SingleAuthor", name = UrlParameter.Optional });
            routes.MapRoute("SingleBook", "Book/SingleBook/{id}", new { controller = "Book", action = "SingleBook", id = UrlParameter.Optional });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion Public Methods
    }
}