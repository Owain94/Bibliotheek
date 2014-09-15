#region

using System.Globalization;
using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using Bibliotheek.Models;
using System;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Controllers
{
    public class AccountController : Controller
    {
        #region Public Methods

        //
        // GET: /Account/Activate 
        [EnableCompression]
        public ActionResult Activate()
        {
            var model = new ActivateModel
            {
                Gender = 0
            };

            var token = string.Empty;
            try
            {
                token = SqlInjection.SafeSqlLiteral(Url.RequestContext.RouteData.Values["id"].ToString());
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
                Response.Redirect("http://66164.ict-lab.nl/", true);
            }

            if (String.IsNullOrEmpty(token) || token.Length != 32)
            {
                Response.Redirect("http://66164.ict-lab.nl/", true);
            }

            model.GetValues(token);

            return View(model);
        }

        //
        // POST: /Account/Activate 
        [HttpPost]
        [EnableCompression]
        public ActionResult Activate(ActivateModel model)
        {
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = string.Empty;
            try
            {
                token = SqlInjection.SafeSqlLiteral(Url.RequestContext.RouteData.Values["id"].ToString());
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
                Response.Redirect("http://66164.ict-lab.nl/", true);
            }

            if (String.IsNullOrEmpty(token) || token.Length != 32)
            {
                Response.Redirect("http://66164.ict-lab.nl/", true);
            }

            model.GetValues(token);
            model.PostalCode =
                Crypt.StringEncrypt(
                    SqlInjection.SafeSqlLiteral(StringManipulation.ToUpperFast(model.PostalCode))
                        .Replace(" ", string.Empty), model.Pepper);
            model.HouseNumber = Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(model.HouseNumber), model.Pepper);

            if (!model.UpdateAccount()) return View("Error");
            Cookies.MakeCookie(model.Mail, model.Id.ToString(CultureInfo.InvariantCulture));
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Login
        [EnableCompression]
        public ActionResult Login()
        {
            // Get view
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [EnableCompression]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View();
            if (model.Login())
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Write(model.Login());
            ViewBag.Error = "Deze inlog gegevens zijn niet bij ons bekend";
            return View();
        }

        //
        // AJAX:
        // GET: /Account/MailCheck
        [EnableCompression]
        public string MailCheck(string input)
        {
            if (ValidateEmail.IsValidEmail(input))
            {
                return RegisterModel.CheckMail(SqlInjection.SafeSqlLiteral(StringManipulation.ToLowerFast(input))) > 0
                    ? "Deze email is al bezet"
                    : "Deze email is nog niet bezet";
            }
            return "Dit is geen geldig email adres";
        }

        //
        // GET: /Account/NewAccount
        [EnableCompression]
        public ActionResult NewAccount()
        {
            // Get view
            return View();
        }

        //
        // POST: /Account/NewAccount
        [HttpPost]
        [EnableCompression]
        public ActionResult NewAccount(RegisterModel model)
        {
            // If model isn't valid return with error messages, otherwise add the user return error or success based on the AddAccount() return
            return ModelState.IsValid ? View(model.AddAccount() ? "Success" : "Error") : View();
        }

        #endregion Public Methods
    }
}