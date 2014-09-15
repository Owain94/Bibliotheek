#region

using System.ComponentModel.DataAnnotations;
using System.Data;
using Bibliotheek.Classes;
using MySql.Data.MySqlClient;

#endregion

namespace Bibliotheek.Models
{
    public class LoginModel
    {
        #region Public Properties

        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email is verplicht")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [Display(Name = "Wachtwoord:")]
        public string Password { get; set; }

        #endregion Public Properties

        public bool Login()
        {
            var email = SqlInjection.SafeSqlLiteral(StringManipulation.ToLowerFast(Email));
            var password = Password;
            var savedPassword = "";
            var savedSalt = "";
            var savedId = "";

            const string result = "SELECT id, password, salt " +
                                  "FROM meok2_bibliotheek_gebruikers " +
                                  "WHERE email = ?";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    showresult.Parameters.Add("email", MySqlDbType.VarChar).Value = email;
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        using (var myDataReader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myDataReader.Read())
                            {
                                savedId = myDataReader.GetValue(1).ToString();
                                savedPassword = myDataReader.GetString(1);
                                savedSalt = myDataReader.GetString(2);
                            }
                        }

                        if (Crypt.ValidatePassword(password, savedPassword, savedSalt))
                        {
                            Cookies.MakeCookie(email, savedId);
                            return true;
                        }
                    }
                    catch (MySqlException)
                    {
                        return false;
                    }
                    finally
                    {
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
            return false;
        }
    }
}