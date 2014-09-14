#region

using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using System.Web.Optimization;

#endregion

namespace Bibliotheek
{
    public static class BundleConfig
    {
        #region Public Methods

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var nullOrderer = new NullOrderer();

            // Replace a default bundle resolver in order to the debugging HTTP-handler can use
            // transformations of the corresponding bundle
            BundleResolver.Current = new CustomBundleResolver();

            var commonStylesBundle = new CustomStyleBundle("~/Bundles/CommonStyles");
            commonStylesBundle.Include(
                "~/Css/bootstrap5152.css",
                "~/Css/responsive5152.css",
                "~/Css/prettyphoto/prettyPhotoaeb9.css",
                "~/Css/main5152.css");
            commonStylesBundle.Orderer = nullOrderer;
            bundles.Add(commonStylesBundle);

            var commonScriptsBundle = new CustomScriptBundle("~/Bundles/CommonScripts");
            commonScriptsBundle.Include(
                "~/Js/jquery.easing.1,3.js",
                "~/Js/prettyphoto/jquery.prettyPhoto.js",
                "~/Js/jquery.liveSearch.js",
                "~/Js/jquery.form.js",
                "~/Js/custom.js");
            commonScriptsBundle.Orderer = nullOrderer;
            bundles.Add(commonScriptsBundle);
        }

        #endregion Public Methods
    }
}