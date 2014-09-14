#region

using System.Web.Http;

#endregion

namespace Bibliotheek
{
    public static class WebApiConfig
    {
        #region Public Methods

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
                );
        }

        #endregion Public Methods
    }
}