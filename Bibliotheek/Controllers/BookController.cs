#region

using System.Globalization;
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
            var model = BindModel(new BookModel());

            return View(model);
        }

        // POST: Book 
        [HttpPost]
        public ActionResult AddBook(BookModel model)
        {
            ViewBag.Error = String.Empty;
            model = BindModel(model);

            if (!ModelState.IsValid) return View(model);
            if (model.AddBook())
            {
                ViewBag.Error = "Het boek is toegevoegd!";
                return View(model);
            }
            ViewBag.Error = "Het boek is niet toegevoegd, probeer het later nog eens!";
            return View(model);
        }

        // GET: Genre 
        public ActionResult Genre(string genre)
        {
            return Content(genre);
        }

        // GET: SingleAuthor 
        public ActionResult SingleAuthor(string name)
        {
            return Content(name);
        }

        // GET: SingleBook 
        public ActionResult SingleBook(string id)
        {
            return Content(id);
        }

        #endregion Public Methods

        private static BookModel BindModel(BookModel model)
        {
            var genreTypes = Enum.GetValues(typeof(BookModel.BookGenres)).Cast<BookModel.BookGenres>();
            model.Genres = from genre in genreTypes
                           select new SelectListItem
                           {
                               Text = BookModel.DictGenre[genre],
                               Value = ((int)genre).ToString(CultureInfo.InvariantCulture)
                           };

            var floorTypes = Enum.GetValues(typeof(BookModel.BookFloors)).Cast<BookModel.BookFloors>();
            model.Floors = from floor in floorTypes
                           select new SelectListItem
                           {
                               Text = BookModel.DictFloors[floor] + " verdieping",
                               Value = ((int)floor).ToString(CultureInfo.InvariantCulture)
                           };

            var rackTypes = Enum.GetValues(typeof(BookModel.BookRacks)).Cast<BookModel.BookRacks>();
            model.Racks = from rack in rackTypes
                          select new SelectListItem
                          {
                              Text = BookModel.DictRacks[rack] + " rek",
                              Value = ((int)rack).ToString(CultureInfo.InvariantCulture)
                          };

            return model;
        }
    }
}