using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Bibliotheek.Classes
{
    public static class CheckAdminRights
    {
        public static bool AdminRights(string id)
        {
            var count = 0;
            // MySQL query 
            var selectStatement = "SELECT admin " +
                                  "FROM meok2_bibliotheek_gebruikers " +
                                  "WHERE id = '" + id + "'";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var selectCommand = new MySqlCommand(selectStatement, empConnection))
                {
                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        using (var myDataReader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myDataReader.Read())
                            {
                                return Convert.ToInt16(myDataReader.GetValue(0)) == 1;
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
            return false;
        }
    }
}