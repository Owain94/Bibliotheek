#region

using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using Bibliotheek.Models;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Controllers
{
    public class AccountController : Controller
    {
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

        //
        // AJAX:
        // GET: /Account/UsernameCheck
        [EnableCompression]
        public string MailCheck(string input)
        {
            return RegisterModel.CheckMail(SqlInjection.SafeSqlLiteral(input)) > 0 ? "Deze email is al bezet" : "";
        }
    }
}