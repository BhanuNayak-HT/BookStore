using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        public BookStoreDBContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public BooksController(BookStoreDBContext context, UserManager<ApplicationUser> _userManager)
        {
            this.context = context;
            this.userManager = _userManager;
        }
        // GET: BooksController
        public async Task<ActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                bool isAdmin = role.Contains("Admin");
                ViewBag.IsAdmin = isAdmin;
            }
            var books = context.books.ToList();
            return View(books);
        }
        //[HttpPost]
        public JsonResult getBooks(int draw, 
            int start,
            int length,
            [FromQuery(Name = "search[value]")] string searchValue,
            [FromQuery(Name = "order[0][column]")] int column,
            [FromQuery(Name = "order[0][dir]")] string dir
            )  
        {
            Console.WriteLine("draw:"+draw);
            Console.WriteLine("start:"+start);
            Console.WriteLine("length:"+length);
            Console.WriteLine("searchValue:" + searchValue);
            var books = context.books.ToList();
                var searchedBooks = String.IsNullOrEmpty(searchValue) ? books : books.Where(b=>b.Title.Contains(searchValue)).ToList();
                Console.WriteLine("length:"+searchedBooks.Count());
            if (column == 1 && dir == "asc")
            {
                var orderedBooks = searchedBooks.OrderBy(b => b.Title).ToList();
                searchedBooks = orderedBooks;
            }
            else if (column == 1 && dir == "desc")
            {
                var orderedBooks = searchedBooks.OrderByDescending(b => b.Title).ToList();
                searchedBooks = orderedBooks;
            }
            var filteredBooks = searchedBooks.Skip(start).Take(length).ToList();
            return Json(new
            {
                draw = draw,
                recordsTotal = books.Count(),
                recordsFiltered = searchedBooks.Count(),
                data = filteredBooks
            });
        }

        // GET: BooksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BooksController/Create
        [Authorize(Roles = "Admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Books book)
        {
            try
            {
                context.books.Add(book);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BooksController/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            Books b = context.books.FirstOrDefault(b => b.Id == id)!;
            if(b == null)
            {
                return RedirectToAction("Index");
            }else
            {
                return View(b);
            }
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(Books book)
        {
            try
            {
                context.Update(book);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {

            return View(book);
            }
        }

        // GET: BooksController/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Delete(int id)
        {
            Books b = context.books.Find(id)!;
            if (b != null)
            {
                context.books.Remove(b);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Index");
            }
        }

        // POST: BooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
