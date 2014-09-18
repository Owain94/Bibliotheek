#region

using System.Web.Mvc;

#endregion

namespace Bibliotheek
{
    public static class FilterConfig
    {
        #region Public Methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion Public Methods
    }
}