#region

using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

#endregion

namespace Bibliotheek.Classes
{
    public static class Message
    {
        #region Public Methods

        #region Public Methods

        // <summary>
        // Send mail 
        // </summary>
        public static String SendMail(string name, string email)
        {
            const bool error = false;
            var body = PopulateBody(name);
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(email);
                mailMessage.From = new MailAddress("noreply@bibliotheek.nl");
                mailMessage.Subject = "Activeer uw account | bibliotheek.nl";
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                var smtpClient = new SmtpClient("145.118.4.13");
                smtpClient.Send(mailMessage);
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
            }
            return error.ToString();
        }

        // <summary>
        // Send mail 
        // </summary>
        public static String SendMail(String name, String email, String subject, String message)
        {
            const bool error = false;
            var body = PopulateBody(name, message);
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add("66164@ict-lab.nl");
                mailMessage.From = new MailAddress(email);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                var smtpClient = new SmtpClient("145.118.4.13");
                smtpClient.Send(mailMessage);
            }
            // ReSharper disable EmptyGeneralCatchClause 
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause 
            {
            }
            return error.ToString();
        }

        #endregion Public Methods

        #endregion Public Methods

        #region Private Methods

        #region Private Methods

        // <summary>
        // Replace placeholders in the email template with vars 
        // </summary>
        private static String PopulateBody(String naam)
        {
            String body;
            String hash;

            using (var md5Hash = MD5.Create())
            {
                hash = Crypt.GetMd5Hash(md5Hash, naam);
            }

            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailActivateTemplate.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{NAAM}", naam);
            body = body.Replace("{URL}", "http://66164.ict-lab.nl/Account/Activate/" + hash + "/");
            return body;
        }

        // <summary>
        // Replace placeholders in the email template with vars 
        // </summary>
        private static String PopulateBody(String naam, String message)
        {
            String body;

            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{NAAM}", naam);
            body = body.Replace("{MESSAGE}", message);
            return body;
        }

        #endregion Private Methods

        #endregion Private Methods
    }
}