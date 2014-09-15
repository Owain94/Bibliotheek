#region

using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Bibliotheek.Models
{
    public class RegisterModel
    {

        #region Public Properties

        [Required(ErrorMessage = "Voormaam is verplicht")]
        [Display(Name = "Voornaam:")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Tussenvoegsel is verplicht")]
        [Display(Name = "Tussenvoegsel:")]
        public string Inclusion { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        [Display(Name = "Achternaam:")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Mail { get; set; }

        #endregion Public Properties

        #region Public Methods

        // <summary>
        // Check if email is in the database already 
        // </summary>
        public static int CheckMail(string mail)
        {
            var count = 0;
            // MySQL query 
            const string countStatement = "SELECT COUNT(*) " +
                                          "FROM meok2_bibliotheek_gebruikers " +
                                          "WHERE email = ?";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var countCommand = new MySqlCommand(countStatement, empConnection))
                {
                    // Bind parameters 
                    countCommand.Parameters.Add("email", MySqlDbType.VarChar).Value = mail;

                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        count = Convert.ToInt32(countCommand.ExecuteScalar());
                    }
                    catch (MySqlException)
                    {
                    }
                    finally
                    {
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
            return count;
        }

        // <summary>
        // Add account to the database and send a mail to the user 
        // </summary>
        public bool AddAccount()
        {
            // Run model through sql prevention and save them to vars 
            var firstName = SqlInjection.SafeSqlLiteral(Firstname);
            var inclusion = SqlInjection.SafeSqlLiteral(Inclusion);
            var lastName = SqlInjection.SafeSqlLiteral(Lastname);
            var mail = SqlInjection.SafeSqlLiteral(StringManipulation.ToLowerFast(Mail));
            var pepper = Crypt.GetRandomSalt();

            // Validate email using regex since HTML5 validation doesn't handle some cases 
            if (!ValidateEmail.IsValidEmail(mail)) return false;

            // MySQL query 
            const string countStatement = "SELECT COUNT(*) " +
                                          "FROM meok2_bibliotheek_gebruikers " +
                                          "WHERE email = ?";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                int count;
                using (var countCommand = new MySqlCommand(countStatement, empConnection))
                {
                    // Bind parameters 
                    countCommand.Parameters.Add("email", MySqlDbType.VarChar).Value = mail;
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        count = Convert.ToInt32(countCommand.ExecuteScalar());
                    }
                    catch (MySqlException)
                    {
                        // MySqlException bail out 
                        return false;
                    }
                    finally
                    {
                        // Make sure to close the connection 
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }

                if (count > 0)
                {
                    // Email already in the database bail out 
                    return false;
                }

                // Insert user in the database 
                const string insertStatement = "INSERT INTO meok2_bibliotheek_gebruikers " +
                                               "(voornaam, tussenvoegsel, achternaam, email, pepper) " +
                                               "VALUES (?, ?, ?, ?, ?)";

                using (var insertCommand = new MySqlCommand(insertStatement, empConnection))
                {
                    // Bind parameters 
                    insertCommand.Parameters.Add("voornaam", MySqlDbType.VarChar).Value =
                        Crypt.StringEncrypt((firstName), pepper);
                    insertCommand.Parameters.Add("tussenvoegsel", MySqlDbType.VarChar).Value =
                        Crypt.StringEncrypt((inclusion), pepper);
                    insertCommand.Parameters.Add("achternaam", MySqlDbType.VarChar).Value =
                        Crypt.StringEncrypt((lastName), pepper);
                    insertCommand.Parameters.Add("email", MySqlDbType.VarChar).Value = mail;
                    insertCommand.Parameters.Add("pepper", MySqlDbType.VarChar).Value = pepper;

                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        insertCommand.ExecuteNonQuery();

                        // Send mail bail out if mail fails 
                        return Message.SendMail(firstName, Mail) == "False";
                    }
                    catch (MySqlException)
                    {
                        // MySqlException bail out 
                        return false;
                    }
                    finally
                    {
                        // Make sure to close the connection 
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
        }

        #endregion Public Methods
    }
}