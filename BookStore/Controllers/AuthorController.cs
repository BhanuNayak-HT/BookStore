using System.Net;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;


namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        public BookStoreDBContext context;
        public IEmailService mailService;
        private readonly UserManager<ApplicationUser> userManager;
        public AuthorController(BookStoreDBContext context, IEmailService _mailService, UserManager<ApplicationUser> _userManager)
        {
            this.context = context;
            this.mailService = _mailService;
            userManager = _userManager;
        }
        public JsonResult getAuthors(
                int draw,
                int start,
                int length,
                [FromQuery(Name = "search[value]")] string searchValue,
                [FromQuery(Name = "order[0][column]")] int column,
                [FromQuery(Name = "order[0][dir]")] string dir
            )
        {
            var authors = context.author.ToList();
            var searchedAuthors = String.IsNullOrEmpty(searchValue) ? authors : authors.Where(b => b.AuthorName.Contains(searchValue)).ToList();
            //Console.WriteLine("length:" + searchedAuthors.Count());
            if (column == 1 && dir == "asc")
            {
                var orderedAuthors = searchedAuthors.OrderBy(b => b.AuthorName).ToList();
                searchedAuthors = orderedAuthors;
            }
            else if (column == 1 && dir == "desc")
            {
                var orderedAuthors = searchedAuthors.OrderByDescending(b => b.AuthorName).ToList();
                searchedAuthors = orderedAuthors;
            }
            var filteredAuthors = searchedAuthors.Skip(start).Take(length).ToList();
            return Json(new
            {
                draw = draw,
                recordsTotal = authors.Count(),
                recordsFiltered = searchedAuthors.Count(),
                data = filteredAuthors
            });
            //List<Author> authors = context.author.ToList();
            //return Json(new { data = authors});
        }
        // GET: AuthorController
        public async Task<ActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                bool isAdmin = role.Contains("Admin");
                ViewBag.IsAdmin = isAdmin; 
            }
            return View(context.author.ToList());
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Author author)
        {
            try
            {
               
                context.author.Add(author);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
          
            Author a = context.author.FirstOrDefault(a => a.AuthorId == id)!;
            return View(a);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(Author author)
        {
            try
            {
                context.Update(author);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> DeleteAsync(int id)
        {
            Author a = context.author.Find(id)!;
            if (a != null)
            {
                context.author.Remove(a);
                await context.SaveChangesAsync();
                //await mailService.SendEmailAsync("xyz@yopmail.com", "something", "some text");
                //var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                //{
                //    Credentials = new NetworkCredential("59d108f240eee1", "062290d86657cb"),
                //    EnableSsl = true
                //};
                //client.Send("abc@mai.25u.com", "abc@mai.25u.com", "Hello world", "testbody");
                //System.Console.WriteLine("Sent");

                var client = new SmtpClient("live.smtp.mailtrap.io", 587)
                {
                    Credentials = new NetworkCredential("api", "3faa22d6c0420b80a94b381bfad001da"),
                    EnableSsl = true
                };
                client.Send("hello@demomailtrap.co", "abc@mai.25u.com", "Hello world", "testbody");
                System.Console.WriteLine("Sent");
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: AuthorController/Delete/5
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
