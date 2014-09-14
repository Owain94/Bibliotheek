#region

using System;
using System.Web;
using System.Web.Security;

#endregion

namespace Bibliotheek.Classes
{
    public abstract class Cookies
    {
        #region Public Methods

        #region Public Methods

        // <summary>
        // Create a HttpOnly cookie 
        // </summary>
        public static void MakeCookie(string name, string savedId)
        {
            var tkt = new FormsAuthenticationTicket(1, name, DateTime.Now,
                DateTime.Now.AddMinutes((44640)), false, savedId);
            var cookiestr = FormsAuthentication.Encrypt(tkt);
            var ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr)
            {
                Expires = tkt.Expiration,
                Path = FormsAuthentication.FormsCookiePath,
                HttpOnly = true
            };
            FormsAuthentication.SetAuthCookie(name, false);
            HttpContext.Current.Response.Cookies.Add(ck);
        }

        #endregion Public Methods

        #endregion Public Methods
    }
}