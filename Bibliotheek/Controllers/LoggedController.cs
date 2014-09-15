#region

using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using System.Web.Mvc;
using System.Web.Security;

#endregion

namespace Bibliotheek.Controllers
{
    public class LoggedController : Controller
    {
        #region Public Methods

        // GET: Logged 
        [EnableCompression]
        public ActionResult Account()
        {
            // Redirect if the user isn't logged in 
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            VerifyAdminRights();
            return View();
        }

        //
        // GET: /Logged/LoggedIn 
        [EnableCompression]
        public ActionResult LoggedIn()
        {
            // Redirect if the user isn't logged in 
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            VerifyAdminRights();
            return View();
        }

        //
        // GET: /Logged/SignOut 
        [EnableCompression]
        public ActionResult SignOut()
        {
            Session["Admin"] = "false";
            FormsAuthentication.SignOut();

            return View();
        }

        #endregion Public Methods

        #region Private Methods

        private void VerifyAdminRights()
        {
            var user = User.Identity as FormsIdentity;
            // ReSharper disable PossibleNullReferenceException 
            var ticket = user.Ticket;
            // ReSharper restore PossibleNullReferenceException 
            if (CheckAdminRights.AdminRights(ticket.UserData))
            {
                Session["Admin"] = "true";
            }
            else
            {
                Session["Admin"] = "false";
            }
        }

        #endregion Private Methods
    }
}