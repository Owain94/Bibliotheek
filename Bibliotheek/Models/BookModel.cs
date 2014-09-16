#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Services.Description;

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
    }
}