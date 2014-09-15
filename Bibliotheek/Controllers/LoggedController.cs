using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bibliotheek.Attributes;
using Bibliotheek.Classes;

namespace Bibliotheek.Controllers
{
    public class LoggedController : Controller
    {
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
    }
}