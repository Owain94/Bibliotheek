#region

using System;
using System.Web;
using System.Web.Security;

#endregion

namespace Bibliotheek.Models
{
    public static class IdentityModel
    {
        #region Public Properties

        public static bool CurrentUserAdmin
        {
            get
            {
                if (!HttpContext.Current.Request.IsAuthenticated) return false;
                var user = HttpContext.Current.User.Identity as FormsIdentity;
                // ReSharper disable PossibleNullReferenceException 
                var ticket = user.Ticket;
                // ReSharper restore PossibleNullReferenceException 
                return ticket.UserData.Split('|')[1] == "1";
            }
        }

        public static int CurrentUserId
        {
            get
            {
                var userId = 0;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name.Split('|')[0]);
                }

                return userId;
            }
        }

        public static bool CurrentUserLoggedIn
        {
            get {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }

        #endregion Public Properties
    }
}