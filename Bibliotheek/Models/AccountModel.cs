using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Bibliotheek.Classes;
using MySql.Data.MySqlClient;

namespace Bibliotheek.Models
{
    public static class AccountModel
    {
        public static string GetAccountDetails()
        {
            var name = String.Empty;
            var address = String.Empty;
            var gender = String.Empty;
            var mail = String.Empty;
            var age = 0;

            // MySQL query Select book in the database 
            const string result = "SELECT voornaam, tussenvoegsel, achternaam, postcode, huisnummer, geslacht, dob, email, pepper " +
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
                                name = Crypt.StringDecrypt(myDataReader.GetString(0), pepper);
                                var tussenvoegsel = myDataReader.GetString(1);
                                gender = myDataReader.GetString(5);
                                var dob = myDataReader.GetDateTime(6);
                                mail = myDataReader.GetString(7);

                                if (!String.IsNullOrEmpty(tussenvoegsel))
                                {
                                    name = name + " " + Crypt.StringDecrypt(tussenvoegsel, pepper);
                                }
                                name = name + " " + Crypt.StringDecrypt(myDataReader.GetString(2), pepper);

                                gender = gender == "0" ? "Man" : "Vrouw";

                                var today = DateTime.Today;
                                age = today.Year - dob.Year;
                                if (dob > today.AddYears(-age)) age--;

                                var request = WebRequest.Create("http://geonl.ict-lab.nl/ajax/checkq1.php?p=" + Crypt.StringDecrypt(myDataReader.GetString(3), pepper));
                                var response = request.GetResponse();
                                var data = response.GetResponseStream();
                                string html;
                                if (data == null) continue;
                                using (var sr = new StreamReader(data))
                                {
                                    html = sr.ReadToEnd();
                                }

                                address = html.Split('"')[3] + " " + Crypt.StringDecrypt(myDataReader.GetString(4), pepper) +
                                       "," + Crypt.StringDecrypt(myDataReader.GetString(3), pepper) + "," + html.Split('"')[15];
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
            return name + "|" + address + "|" + gender + "|" + age + "|" + mail;
        }
    }
}