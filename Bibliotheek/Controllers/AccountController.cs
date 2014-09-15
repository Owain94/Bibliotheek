#region

using System.Drawing.Imaging;
using System.Web.Security;
using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using Bibliotheek.Models;
using System;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

#endregion

namespace Bibliotheek.Controllers
{
    public class AccountController : Controller
    {
        #region Public Methods

        //
        // GET: /Account/Register
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
        // POST: /Account/Register
        [HttpPost]
        [EnableCompression]
        public ActionResult Activate(ActivateModel model)
        {
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
            model.Password = Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(model.Password), model.Pepper);

            if (model.UpdateAccount())
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

        //
        // AJAX:
        // GET: /Account/UsernameCheck
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
        // GET: /Account/
        [EnableCompression]
        public ActionResult NewAccount()
        {
            // Get view
            return View();
        }

        //
        // POST: /Account/
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