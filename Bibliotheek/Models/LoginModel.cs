﻿#region

using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

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

        #region Private Properties

        private int Admin { get; set; }

        #endregion Private Properties

        #region Public Methods

        public bool Login()
        {
            var email = SqlInjection.SafeSqlLiteral(StringManipulation.ToLowerFast(Email));
            var password = Password;
            var savedPassword = String.Empty;
            var savedSalt = String.Empty;
            var savedId = String.Empty;

            // MySQL query 
            const string result = "SELECT id, password, salt, admin " +
                                  "FROM meok2_bibliotheek_gebruikers " +
                                  "WHERE email = ?";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    // Bind parameters 
                    showresult.Parameters.Add("email", MySqlDbType.VarChar).Value = email;
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var myDataReader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myDataReader.Read())
                            {
                                // Save the values 
                                savedId = myDataReader.GetValue(0).ToString();
                                savedPassword = myDataReader.GetString(1);
                                savedSalt = myDataReader.GetString(2);
                                Admin = Convert.ToInt16(myDataReader.GetValue(3));
                            }
                        }

                        // Hash the password and check if the hash is the same as the saved password 
                        if (Crypt.ValidatePassword(password, savedPassword, savedSalt))
                        {
                            Cookies.MakeCookie(email, savedId, Admin.ToString(CultureInfo.InvariantCulture));
                            return true;
                        }
                    }
                    catch (MySqlException)
                    {
                        // MySqlException bail out 
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

        #endregion Public Methods
    }
}
