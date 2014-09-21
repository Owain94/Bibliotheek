#region

using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Cryptography;

#endregion

namespace Bibliotheek.Models
{
    public class ActivateModel
    {
        #region Public Properties

        public int Id { get; private set; }

        [Display(Name = "Voornaam:")]
        public string Firstname { get; private set; }

        [Display(Name = "Tussenvoegsel:")]
        public string Affix { get; private set; }

        [Display(Name = "Achternaam:")]
        public string Lastname { get; private set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Mail { get; private set; }

        public string Pepper { get; private set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode:")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Display(Name = "Huisnummer:")]
        public string HouseNumber { get; set; }

        [Display(Name = "Geslacht:")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        [Display(Name = "Geboortedatum:")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [Display(Name = "Wachtwoord:")]
        public string Password { get; set; }

        #endregion Public Properties

        #region Public Methods

        // <summary>
        // Check if the account is already activated 
        // </summary>
        public static bool CheckAccount(string token)
        {
            // MySQL query 
            const string result = "SELECT id, salt, email " +
                                  "FROM meok2_bibliotheek_gebruikers";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var reader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                using (var md5Hash = MD5.Create())
                                {
                                    // Check if the MD5 hash mathes 
                                    if (!Crypt.VerifyMd5Hash(md5Hash, reader.GetValue(2).ToString(), token)) continue;

                                    var id = reader.GetValue(0).ToString();

                                    if (id == "-1") continue;
                                    // Check if salt is not empty or not null 
                                    if (!String.IsNullOrEmpty(reader.GetValue(1).ToString()))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
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
            return true;
        }

        // <summary>
        // Get values for the giving account 
        // </summary>
        public void GetValues(string token)
        {
            // MySQL query 
            const string result = "SELECT id, voornaam, tussenvoegsel, achternaam, email, pepper " +
                                  "FROM meok2_bibliotheek_gebruikers";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var reader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                // Check if the MD5 hash mathes 
                                using (var md5Hash = MD5.Create())
                                {
                                    if (!Crypt.VerifyMd5Hash(md5Hash, reader.GetValue(4).ToString(), token)) continue;
                                    // Save the values 
                                    var id = reader.GetValue(0).ToString();
                                    var pepper = reader.GetValue(5).ToString();

                                    if (id == "-1") continue;
                                    // Save values to the model 
                                    Id = Convert.ToInt16(id);
                                    Firstname =
                                        SqlInjection.SafeSqlLiteralRevert(
                                            Crypt.StringDecrypt(reader.GetValue(1).ToString(), pepper));
                                    Affix =
                                        SqlInjection.SafeSqlLiteralRevert(
                                            Crypt.StringDecrypt(reader.GetValue(2).ToString(), pepper));
                                    Lastname =
                                        SqlInjection.SafeSqlLiteralRevert(
                                            Crypt.StringDecrypt(reader.GetValue(3).ToString(), pepper));
                                    Mail = SqlInjection.SafeSqlLiteralRevert(reader.GetValue(4).ToString());
                                    Pepper = pepper;
                                }
                            }
                        }
                    }
                    catch (MySqlException)
                    {
                        // MySqlException bail out 
                    }
                    finally
                    {
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
        }

        // <summary>
        // Update account from the activate page 
        // </summary>
        public bool UpdateAccount()
        {
            // MySQL query 
            const string result = "UPDATE meok2_bibliotheek_gebruikers " +
                                  "SET postcode = ?, " +
                                  "huisnummer = ?, " +
                                  "geslacht = ?, " +
                                  "dob = ?, " +
                                  "password = ?, " +
                                  "salt = ? " +
                                  "WHERE id = ?";

            var salt = Crypt.GetRandomSalt();
            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    // Bind parameters 
                    showresult.Parameters.Add("postcode", MySqlDbType.VarChar).Value = PostalCode;
                    showresult.Parameters.Add("huisnummer", MySqlDbType.VarChar).Value = HouseNumber;
                    showresult.Parameters.Add("geslacht", MySqlDbType.VarChar).Value = Gender;
                    showresult.Parameters.Add("dob", MySqlDbType.Date).Value = StringManipulation.DateTimeToMySql(Dob);
                    showresult.Parameters.Add("password", MySqlDbType.VarChar).Value = Crypt.HashPassword(Password, salt);
                    showresult.Parameters.Add("salt", MySqlDbType.VarChar).Value = salt;
                    showresult.Parameters.Add("id", MySqlDbType.Int16).Value = Id;

                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        showresult.ExecuteNonQuery();
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
            return true;
        }

        #endregion Public Methods
    }
}