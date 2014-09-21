#region

using System.Web;
using Bibliotheek.Attributes;
using Bibliotheek.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace Bibliotheek.Controllers
{
    public class BookController : Controller
    {
        #region Public Methods

        // GET: Book 
        [EnableCompression]
        public ActionResult AddBook()
        {
            // Redirect is the user isn't an admin 
            if (!IdentityModel.CurrentUserAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = BindModel(new BookModel());

            return View(model);
        }

        // POST: Book 
        [HttpPost]
        [EnableCompression]
        public ActionResult AddBook(BookModel model)
        {
            // Redirect is the user isn't an admin 
            if (!IdentityModel.CurrentUserAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

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

        // GET: AllBooks 
        [EnableCompression]
        public ActionResult AllBooks()
        {
            // Redirect if the user isn't logged in
            if (!IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Genre 
        [EnableCompression]
        public ActionResult Genre(string genre)
        {
            // Redirect if the user isn't logged in
            if (!IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: SearchBooks 
        [EnableCompression]
        public ActionResult SearchBooks(string searchTerm)
        {
            // Redirect if the user isn't logged in
            if (!IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: SingleAuthor 
        [EnableCompression]
        public ActionResult SingleAuthor(string name)
        {
            // Redirect if the user isn't logged in
            if (!IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: SingleBook 
        [EnableCompression]
        public ActionResult SingleBook(string id)
        {
            // Redirect if the user isn't logged in
            if (!IdentityModel.CurrentUserLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #endregion Public Methods

        #region Private Methods

        private static BookModel BindModel(BookModel model)
        {
            var genreTypes = Enum.GetValues(typeof (BookModel.BookGenres)).Cast<BookModel.BookGenres>();
            model.Genres = from genre in genreTypes
                select new SelectListItem
                {
                    Text = BookModel.DictGenre[genre],
                    Value = ((int) genre).ToString(CultureInfo.InvariantCulture)
                };

            var floorTypes = Enum.GetValues(typeof (BookModel.BookFloors)).Cast<BookModel.BookFloors>();
            model.Floors = from floor in floorTypes
                select new SelectListItem
                {
                    Text = BookModel.DictFloors[floor] + " verdieping",
                    Value = ((int) floor).ToString(CultureInfo.InvariantCulture)
                };

            var rackTypes = Enum.GetValues(typeof (BookModel.BookRacks)).Cast<BookModel.BookRacks>();
            model.Racks = from rack in rackTypes
                select new SelectListItem
                {
                    Text = BookModel.DictRacks[rack] + " rek",
                    Value = ((int) rack).ToString(CultureInfo.InvariantCulture)
                };

            return model;
        }

        #endregion Private Methods
    }
}