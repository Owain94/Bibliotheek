#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

#endregion

namespace Bibliotheek
{
    public static class WebApiConfig
    {
        #region Public Methods

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        #endregion Public Methods
    }
}