#region

using System.Collections.Generic;
using System.Globalization;
using Bibliotheek.Attributes;
using Bibliotheek.Models;
using System;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Controllers
{
    public class BookController : Controller
    {
        #region Public Methods

        // GET: Book 
        public ActionResult AddBook()
        {
            var dictGenre = new Dictionary<BookModel.BookGenres, string> {
                { BookModel.BookGenres.Literatuur, "Literatuur & Romans" },
                { BookModel.BookGenres.Thrillers, "Thrillers" },
                { BookModel.BookGenres.Kind, "Kind & Jeugd" },
                { BookModel.BookGenres.Eten, "Eten & Koken" },
                { BookModel.BookGenres.Gezondheid, "Gezondheid" },
                { BookModel.BookGenres.Psychologie, "Psychologie" },
                { BookModel.BookGenres.Fantasy, "Fantasy" },
                { BookModel.BookGenres.Young, "Young Adults" },
                { BookModel.BookGenres.Reizen, "Reizen & Talen" },
                { BookModel.BookGenres.Sport, "Sport & Hobby" },
                { BookModel.BookGenres.Kunst, "Kunst & Cultuur" },
                { BookModel.BookGenres.Geschiedenis, "Geschiedenis & Politiek" },
                { BookModel.BookGenres.Studieboeken, "Studieboeken" },
                { BookModel.BookGenres.Managementboeken, "Managementboeken" },
                { BookModel.BookGenres.Computer, "Computer" },
                { BookModel.BookGenres.Religie, "Religie" },
                { BookModel.BookGenres.Spiritualiteit, "Spiritualiteit" },
                { BookModel.BookGenres.School, "School & Examen" },
                { BookModel.BookGenres.Stripboeken, "Stripboeken" },
                { BookModel.BookGenres.Wonen, "Wonen & Tuinieren" }
            };

            var dictFloors = new Dictionary<BookModel.BookFloors, string> {
                { BookModel.BookFloors.First, "1ste" },
                { BookModel.BookFloors.Second, "2de" },
                { BookModel.BookFloors.Third, "3de" }
            };

            var dictRacks = new Dictionary<BookModel.BookRacks, string> {
                { BookModel.BookRacks.First, "1ste" },
                { BookModel.BookRacks.Second, "2de" },
                { BookModel.BookRacks.Third, "3de" },
                { BookModel.BookRacks.Fourth, "4de" },
                { BookModel.BookRacks.Fifth, "5de" },
                { BookModel.BookRacks.Sixth, "6de" },
                { BookModel.BookRacks.Seventh, "7de" },
                { BookModel.BookRacks.Eighth, "8ste" },
                { BookModel.BookRacks.Ninth, "9de" },
                { BookModel.BookRacks.Tenth, "10de" },
                { BookModel.BookRacks.Eleventh, "11de" },
                { BookModel.BookRacks.Twelfth, "12de" },
                { BookModel.BookRacks.Thirteenth, "13de" },
                { BookModel.BookRacks.Fourteenth, "14de" },
                { BookModel.BookRacks.Fifteenth, "15de" }
            };

            var model = new BookModel();

            var genreTypes = Enum.GetValues(typeof(BookModel.BookGenres)).Cast<BookModel.BookGenres>();
            model.Genres = from genre in genreTypes
                           select new SelectListItem
                           {
                               Text = dictGenre[genre],
                               Value = ((int)genre).ToString(CultureInfo.InvariantCulture)
                           };

            var floorTypes = Enum.GetValues(typeof(BookModel.BookFloors)).Cast<BookModel.BookFloors>();
            model.Floors = from floor in floorTypes
                           select new SelectListItem
                           {
                               Text = dictFloors[floor] + " verdieping",
                               Value = ((int)floor).ToString(CultureInfo.InvariantCulture)
                           };

            var rackTypes = Enum.GetValues(typeof(BookModel.BookRacks)).Cast<BookModel.BookRacks>();
            model.Racks = from rack in rackTypes
                           select new SelectListItem
                           {
                               Text = dictRacks[rack] + " rek",
                               Value = ((int)rack).ToString(CultureInfo.InvariantCulture)
                           };

            return View(model);
        }

        // POST: Book 
        [HttpPost]
        [EnableCompression]
        public ActionResult AddBook(BookModel model)
        {
            return View();
        }

        #endregion Public Methods
    }
}