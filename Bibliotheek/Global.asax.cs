#region

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using Bibliotheek.Classes;

#endregion

namespace Bibliotheek
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801 
    public class MvcApplication : HttpApplication
    {
        #region Protected Methods

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            var viewEngine = new RazorViewEngine();
            viewEngine.ViewLocationCache = new TwoLevelViewCache(viewEngine.ViewLocationCache);
            ViewEngines.Engines.Add(viewEngine);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelMetadataProviders.Current = new CachedDataAnnotationsModelMetadataProvider();

            MvcHandler.DisableMvcResponseHeader = true;

            ScriptManager.ScriptResourceMapping.AddDefinition("jQuery", new ScriptResourceDefinition
            {
                Path = "~/Js/jquery-2.1.1.js",
                DebugPath = "~/Js/jquery-2.1.1.js",
                CdnPath = "https://code.jquery.com/jquery-2.1.1.min.js",
                CdnDebugPath = "https://code.jquery.com/jquery-2.1.1.min.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
        }

        #endregion Protected Methods
    }
}