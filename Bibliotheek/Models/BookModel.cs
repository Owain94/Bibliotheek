#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using Message = System.Web.Services.Description.Message;

#endregion

namespace Bibliotheek.Models
{
    public class BookModel
    {
        #region Public Properties

        [Required(ErrorMessage = "Schrijver is verplicht")]
        [Display(Name = "Schrijver:")]
        public string Author { get; set; }

        [Display(Name = "Locatie: ")]
        public int Floor { get; set; }
        public IEnumerable<SelectListItem> Floors { get; set; }

        [Display(Name = "Genre: ")]
        public int Genre { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }

        [Display(Name = "ISBN: ")]
        [Required(ErrorMessage = "ISBN is verplicht")]
        public string Isbn { get; set; }

        public int Rack { get; set; }

        public IEnumerable<SelectListItem> Racks { get; set; }

        [Display(Name = "Titel: ")]
        [Required(ErrorMessage = "Titel is verplicht")]
        public string Title { get; set; }

        #endregion Public Properties

        public enum BookGenres
        {
            Literatuur = 1,
            Thrillers = 2,
            Kind = 3,
            Eten = 4,
            Gezondheid = 5,
            Psychologie = 6,
            Fantasy = 7,
            Young = 8,
            Reizen = 9,
            Sport = 10,
            Kunst = 11,
            Geschiedenis = 12,
            Studieboeken = 13,
            Managementboeken = 14,
            Computer = 15,
            Religie = 16,
            Spiritualiteit = 17,
            School = 18,
            Stripboeken = 19,
            Wonen = 20
        }
        public enum BookFloors
        {
            First = 1,
            Second = 2,
            Third = 3
        }
        public enum BookRacks
        {
            First  = 1,
            Second   = 2,
            Third   = 3,
            Fourth   = 4,
            Fifth   = 5,
            Sixth   = 6,
            Seventh   = 7,
            Eighth   = 8,
            Ninth   = 9,
            Tenth   = 10,
            Eleventh   = 11,
            Twelfth   = 12,
            Thirteenth   = 13,
            Fourteenth = 14,
            Fifteenth  = 15,
        }

        // <summary>
        // Add book to the database
        // </summary>
        public bool AddBook()
        {
            // Run model through sql prevention and save them to vars 
            var title = SqlInjection.SafeSqlLiteral(Title);
            var author = SqlInjection.SafeSqlLiteral(Author);
            var genre = Genre;
            var isbn = SqlInjection.SafeSqlLiteral(Isbn);
            var floor = Floor;
            var rack = Rack;
            var dateAdded = StringManipulation.DateTimeToMySql(DateTime.Now);

            // MySQL query 
            // Insert book in the database 
            const string insertStatement = "INSERT INTO meok2_bibliotheek_boeken " +
                                           "(titel, auteur, genre, isbn, verdieping, rek, dateadded) " +
                                           "VALUES (?, ?, ?, ?, ?, ?, ?)";

            using (var empConnection = DatabaseConnection.DatabaseConnect())
            {
                using (var insertCommand = new MySqlCommand(insertStatement, empConnection))
                {
                    // Bind parameters 
                    insertCommand.Parameters.Add("titel", MySqlDbType.VarChar).Value = title;
                    insertCommand.Parameters.Add("auteur", MySqlDbType.VarChar).Value = author;
                    insertCommand.Parameters.Add("genre", MySqlDbType.Int16).Value = genre;
                    insertCommand.Parameters.Add("isbn", MySqlDbType.VarChar).Value = isbn;
                    insertCommand.Parameters.Add("verdieping", MySqlDbType.Int16).Value = floor;
                    insertCommand.Parameters.Add("rek", MySqlDbType.Int16).Value = rack;
                    insertCommand.Parameters.Add("dateadded", MySqlDbType.Date).Value = dateAdded;

                    try
                    {
                        DatabaseConnection.DatabaseOpen(empConnection);
                        // Execute command 
                        insertCommand.ExecuteNonQuery();

                        // Return 
                        return true;
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
    }
}