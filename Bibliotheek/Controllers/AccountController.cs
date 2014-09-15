#region

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

            Response.Write("ID: " + model.Id + "<br />");
            Response.Write("Voornaam: " + model.Firstname + "<br />");
            Response.Write("Tussenvoegsel: " + model.Inclusion + "<br />");
            Response.Write("Achternaam: " + model.Lastname + "<br />");
            Response.Write("Email: " + model.Mail + "<br />");
            Response.Write("Postcode: " + Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(StringManipulation.ToUpperFast(model.PostalCode)).Replace(" ", string.Empty), model.Pepper) + "<br />");
            Response.Write("Huisnummer: " + Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(model.HouseNumber), model.Pepper) + "<br />");
            Response.Write("Geslacht: " + model.Gender + "<br />");
            Response.Write("Geboortedatum: " + StringManipulation.DateTimeToMySql(model.DOB) + "<br />");
            Response.Write("Wachtwoord: " + Crypt.StringEncrypt(SqlInjection.SafeSqlLiteral(model.Password), model.Pepper) + "<br />");
            Response.Write("Pepper: " + model.Pepper + "<br />");

            return View(model);
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