#region

using System.Globalization;
using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;

#endregion

namespace Bibliotheek.Models
{
    public static class AccountModel
    {
        #region Public Methods

        // <summary>
        // Select all users from database 
        // </summary>
        public static List<String> AllUsers()
        {
            // Initial vars 
            var list = new List<String>();

            // MySQL query 
            const string result = "SELECT id, voornaam, tussenvoegsel, achternaam, pepper, email " +
                                  "FROM meok2_bibliotheek_gebruikers";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var myDataReader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myDataReader.Read())
                            {
                                // Save the values 
                                var id = myDataReader.GetString(0);
                                var pepper = myDataReader.GetString(4);

                                var name =
                                    SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(myDataReader.GetString(1),
                                        pepper));
                                var affix = myDataReader.GetString(2);
                                var lastname =
                                    SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(myDataReader.GetString(3),
                                        pepper));
                                var email = myDataReader.GetString(5);

                                if (!String.IsNullOrEmpty(affix))
                                {
                                    name = name + " " +
                                           SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(affix, pepper));
                                }
                                name = name + " " + lastname;

                                list.Add(id);
                                list.Add(name);
                                list.Add(email);
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
            return list;
        }

        public static List<String> GetAccountDetails()
        {
            // Initial vars 
            var list = new List<String>();

            // MySQL query Select book in the database 
            const string result =
                "SELECT voornaam, tussenvoegsel, achternaam, postcode, huisnummer, geslacht, dob, email, pepper " +
                "FROM meok2_bibliotheek_gebruikers " +
                "WHERE email = ?";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var showresult = new MySqlCommand(result, empConnection))
                {
                    showresult.Parameters.Add("id", MySqlDbType.VarChar).Value = IdentityModel.CurrentUserId;
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var myDataReader = showresult.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myDataReader.Read())
                            {
                                // Save the values 
                                var pepper = myDataReader.GetString(8);
                                var name = SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(myDataReader.GetString(0),
                                    pepper));
                                var affix = myDataReader.GetString(1);
                                var gender = myDataReader.GetString(5);
                                var dob = myDataReader.GetDateTime(6);
                                var mail = myDataReader.GetString(7);

                                if (!String.IsNullOrEmpty(affix))
                                {
                                    name = name + " " +
                                           SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(affix, pepper));
                                }
                                name = name + " " +
                                       SqlInjection.SafeSqlLiteralRevert(Crypt.StringDecrypt(myDataReader.GetString(2),
                                           pepper));

                                var today = DateTime.Today;
                                var age = today.Year - dob.Year;
                                if (dob > today.AddYears(-age)) age--;

                                var request =
                                    WebRequest.Create("http://geonl.ict-lab.nl/ajax/checkq1.php?p=" +
                                                      SqlInjection.SafeSqlLiteralRevert(
                                                          Crypt.StringDecrypt(myDataReader.GetString(3), pepper)));
                                var response = request.GetResponse();
                                var data = response.GetResponseStream();
                                string html;
                                if (data == null) continue;
                                using (var sr = new StreamReader(data))
                                {
                                    html = sr.ReadToEnd();
                                }

                                var address = html.Split('"')[3] + " " +
                                                 SqlInjection.SafeSqlLiteralRevert(
                                                     Crypt.StringDecrypt(myDataReader.GetString(4), pepper)) +
                                                 "," +
                                                 SqlInjection.SafeSqlLiteralRevert(
                                                     Crypt.StringDecrypt(myDataReader.GetString(3), pepper)) + "," +
                                                 html.Split('"')[15];
                                
                                list.Add(name);
                                list.Add(address);
                                list.Add(gender == "0" ? "Man" : "Vrouw");
                                list.Add(age.ToString(CultureInfo.InvariantCulture));
                                list.Add(mail);
                            }
                        }
                    }
                    catch (MySqlException)
                    {
                        // MySqlException 
                    }
                    finally
                    {
                        DatabaseConnection.DatabaseClose(empConnection);
                    }
                }
            }
            return list;
        }

        #endregion Public Methods
    }
}