#region

using Bibliotheek.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Models
{
    public class BookModel
    {
        #region Public Fields

        public static readonly Dictionary<BookFloors, string> DictFloors = new Dictionary<BookFloors, string>
        {
            {BookFloors.First, "1ste"},
            {BookFloors.Second, "2de"},
            {BookFloors.Third, "3de"}
        };

        public static readonly Dictionary<BookGenres, string> DictGenre = new Dictionary<BookGenres, string>
        {
            {BookGenres.Literatuur, "Literatuur & Romans"},
            {BookGenres.Thrillers, "Thrillers"},
            {BookGenres.Kind, "Kind & Jeugd"},
            {BookGenres.Eten, "Eten & Koken"},
            {BookGenres.Gezondheid, "Gezondheid"},
            {BookGenres.Psychologie, "Psychologie"},
            {BookGenres.Fantasy, "Fantasy"},
            {BookGenres.Young, "Young Adults"},
            {BookGenres.Reizen, "Reizen & Talen"},
            {BookGenres.Sport, "Sport & Hobby"},
            {BookGenres.Kunst, "Kunst & Cultuur"},
            {BookGenres.Geschiedenis, "Geschiedenis & Politiek"},
            {BookGenres.Studieboeken, "Studieboeken"},
            {BookGenres.Managementboeken, "Managementboeken"},
            {BookGenres.Computer, "Computer"},
            {BookGenres.Religie, "Religie"},
            {BookGenres.Spiritualiteit, "Spiritualiteit"},
            {BookGenres.School, "School & Examen"},
            {BookGenres.Stripboeken, "Stripboeken"},
            {BookGenres.Wonen, "Wonen & Tuinieren"}
        };

        public static readonly Dictionary<int, string> DictGenreFromInt = new Dictionary<int, string>
        {
            {1, "Literatuur & Romans"},
            {2, "Thrillers"},
            {3, "Kind & Jeugd"},
            {4, "Eten & Koken"},
            {5, "Gezondheid"},
            {6, "Psychologie"},
            {7, "Fantasy"},
            {8, "Young Adults"},
            {9, "Reizen & Talen"},
            {10, "Sport & Hobby"},
            {11, "Kunst & Cultuur"},
            {12, "Geschiedenis & Politiek"},
            {13, "Studieboeken"},
            {14, "Managementboeken"},
            {15, "Computer"},
            {16, "Religie"},
            {17, "Spiritualiteit"},
            {18, "School & Examen"},
            {19, "Stripboeken"},
            {20, "Wonen & Tuinieren"}
        };

        public static readonly Dictionary<BookRacks, string> DictRacks = new Dictionary<BookRacks, string>
        {
            {BookRacks.First, "1ste"},
            {BookRacks.Second, "2de"},
            {BookRacks.Third, "3de"},
            {BookRacks.Fourth, "4de"},
            {BookRacks.Fifth, "5de"},
            {BookRacks.Sixth, "6de"},
            {BookRacks.Seventh, "7de"},
            {BookRacks.Eighth, "8ste"},
            {BookRacks.Ninth, "9de"},
            {BookRacks.Tenth, "10de"},
            {BookRacks.Eleventh, "11de"},
            {BookRacks.Twelfth, "12de"},
            {BookRacks.Thirteenth, "13de"},
            {BookRacks.Fourteenth, "14de"},
            {BookRacks.Fifteenth, "15de"}
        };

        #endregion Public Fields

        #region Public Enums

        public enum BookFloors
        {
            First = 1,
            Second = 2,
            Third = 3
        }

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

        public enum BookRacks
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4,
            Fifth = 5,
            Sixth = 6,
            Seventh = 7,
            Eighth = 8,
            Ninth = 9,
            Tenth = 10,
            Eleventh = 11,
            Twelfth = 12,
            Thirteenth = 13,
            Fourteenth = 14,
            Fifteenth = 15,
        }

        #endregion Public Enums

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

        #region Public Methods

        public static string GetNewBooks()
        {
            var id = String.Empty;
            var title = String.Empty;
            var author = String.Empty;
            var genre = String.Empty;
            var date = String.Empty;
            // MySQL query Select book in the database 
            const string result = "SELECT id, titel, auteur, genre, dateadded " +
                                  "FROM meok2_bibliotheek_boeken " +
                                  "ORDER BY id DESC " +
                                  "LIMIT 5";

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
                                id = id + myDataReader.GetString(0) + "|";
                                title = title + myDataReader.GetString(1) + "|";
                                author = author + myDataReader.GetString(2) + "|";
                                genre = genre + myDataReader.GetInt16(3) + "|";
                                date = date + myDataReader.GetDateTime(4).ToString("d MMM yyyy") + "|";
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
            return id + title + author + genre + date;
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

            // MySQL query Insert book in the database 
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

        #endregion Public Methods
    }
}