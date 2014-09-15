using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bibliotheek.Attributes;

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
            return View();
        }

        //
        // GET: /Logged/SignOut
        [EnableCompression]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return View();
        }
    }
}