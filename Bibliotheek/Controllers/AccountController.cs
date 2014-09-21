using System.Web.UI.WebControls;
using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using Bibliotheek.Models;
using System;
using System.Globalization;
using System.Web.Mvc;

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
            // Redirect if the user is logged in already 
            if (IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Account", "Logged");
            }

            var model = new ActivateModel
            {
                // Set default 
                Gender = 0
            };

            string token;
            try
            {
                // Get the token from the RouteData 
                token = SqlInjection.SafeSqlLiteral(Url.RequestContext.RouteData.Values["id"].ToString());
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
                return RedirectToAction("Index", "Home");
            }

            // Redirect if the token is invalid or missing 
            if (String.IsNullOrEmpty(token) || token.Length != 32)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ActivateModel.CheckAccount(token)) return RedirectToAction("Account", "Logged");

            // Get values form the database 
            model.GetValues(token);

            return View(model);
        }

        //
        // POST: /Account/Activate 
        [HttpPost]
        [EnableCompression]
        public ActionResult Activate(ActivateModel model)
        {
            string token;
            try
            {
                // Get the token from the RouteData 
                token = SqlInjection.SafeSqlLiteral(Url.RequestContext.RouteData.Values["id"].ToString());
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
                return RedirectToAction("Index", "Home");
            }

            if (String.IsNullOrEmpty(token) || token.Length != 32)
            {
                return RedirectToAction("Index", "Home");
            }
            // Load in values from database 
            model.GetValues(token);

            // Make Postal code upperCase, remove spaces and encrypt the string 
            model.PostalCode =
                Crypt.StringEncrypt(
                    SqlInjection.SafeSqlLiteral(StringManipulation.ToUpperFast(model.PostalCode))
                        .Replace(" ", string.Empty), model.Pepper);
            model.HouseNumber = Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(model.HouseNumber), model.Pepper);

            // If UpdateAccount fails show error page 
            if (!model.UpdateAccount()) return View("Error");
            // Make cookie for user 
            Cookies.MakeCookie(model.Mail, model.Id.ToString(CultureInfo.InvariantCulture), "0");
            return RedirectToAction("Account", "Logged");
        }

        //
        // GET: /Account/AllAccounts 
        [EnableCompression]
        public ActionResult AllAccounts()
        {
            // Redirect if the user isn't an admin 
            if (!IdentityModel.CurrentUserAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            // Get view
            return View(new RegisterModel());
        }

        //
        // GET: /Account/Login 
        [EnableCompression]
        public ActionResult Login()
        {
            // Redirect if the user is logged in already 
            if (IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Account", "Logged");
            }
            // Get view 
            return View(new LoginModel());
        }

        //
        // POST: /Account/Login 
        [HttpPost]
        [EnableCompression]
        public ActionResult Login(LoginModel model)
        {
            // If modelState is invalid return the View 
            if (!ModelState.IsValid) return View(model);
            if (model.Login())
            {
                // If email and password are correct redirect user 
                return RedirectToAction("LoggedIn", "Logged");
            }
            // Show error if the username and password are wrong 
            ViewBag.Error = "Deze inlog gegevens zijn niet bij ons bekend";
            return View(model);
        }

        //
        // AJAX:
        // GET: /Account/MailCheck
        [EnableCompression]
        public string MailCheck(string input)
        {
            // Validate email
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
            // Redirect if the user isn't an admin 
            if (!IdentityModel.CurrentUserAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            // Get view
            return View(new RegisterModel());
        }

        //
        // POST: /Account/NewAccount
        [HttpPost]
        [EnableCompression]
        public ActionResult NewAccount(RegisterModel model)
        {
            // Redirect if the user isn't an admin 
            if (!IdentityModel.CurrentUserAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            // If model isn't valid return with error messages, otherwise add the user return error or success based on the AddAccount() return
            return ModelState.IsValid ? View(model.AddAccount() ? "Success" : "Error") : View();
        }

        #endregion Public Methods
    }
}