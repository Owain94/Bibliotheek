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

        public int Id { get; set; }

        [Display(Name = "Voornaam:")]
        public string Firstname { get; set; }

        [Display(Name = "Tussenvoegsel:")]
        public string Inclusion { get; set; }

        [Display(Name = "Achternaam:")]
        public string Lastname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Mail { get; set; }

        public string Pepper { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode:")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Display(Name = "Huisnummer:")]
        public string HouseNumber { get; set; }

        [Display(Name = "Geslacht:")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        [Display(Name = "Geboortedatum:")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [Display(Name = "Wachtwoord:")]
        public string Password { get; set; }

        #endregion Public Properties

        #region Public Methods

        // <summary>
        // Check if email is in the database already 
        // </summary>
        public void GetValues(string token)
        {
            const string result = "SELECT id, voornaam, tussenvoegsel, achternaam, email, pepper " +
                                  "FROM meok2_bibliotheek_gebruikers";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        using (var reader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                using (var md5Hash = MD5.Create())
                                {
                                    if (!Crypt.VerifyMd5Hash(md5Hash, reader.GetValue(4).ToString(), token)) continue;

                                    var id = reader.GetValue(0).ToString();
                                    var pepper = reader.GetValue(5).ToString();

                                    if (id == "-1") continue;
                                    Id = Convert.ToInt16(id);
                                    Firstname =
                                        SqlInjection.SafeSqlLiteralRevert(
                                            Crypt.StringDecrypt(reader.GetValue(1).ToString(), pepper));
                                    Inclusion =
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
                    // ReSharper disable EmptyGeneralCatchClause 
                    catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause 
                    {
                    }
                    finally
                    {
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
        }

        #endregion Public Methods
    }
}