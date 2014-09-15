#region

using Bibliotheek.Attributes;
using Bibliotheek.Classes;
using Bibliotheek.Models;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Controllers
{
    public class HomeController : Controller
    {
        #region Public Methods

        //
        // GET: /Home/About 
        [EnableCompression]
        public ActionResult About()
        {
            // Get view 
            return View();
        }

        //
        // GET: /Home/Contact 
        [EnableCompression]
        public ActionResult Contact()
        {
            // Get view 
            return View();
        }

        //
        // POST: /Home/Contact 
        [HttpPost]
        [EnableCompression]
        public ActionResult Contact(ContactModel model)
        {
            // If model isn't valid return with error messages, otherwise send the mail return error
            // or success based on the SendMail() return
            return !ModelState.IsValid
                ? View()
                : View(Message.SendMail(model.Name, model.Email, model.Subject, model.Message) == "False"
                    ? "Success"
                    : "Error");
        }

        //
        // GET: /Home/Index 
        [EnableCompression]
        public ActionResult Index()
        {
            // Get view 
            return View();
        }

        //
        // AJAX:
        // GET: /Home/MailCheck
        [EnableCompression]
        public string MailCheck(string input)
        {
            return ValidateEmail.IsValidEmail(input) ? "true" : "false";
        }

        #endregion Public Methods
    }
}