using FoodOnTheGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodDbContext dbContext;
        public HomeController(FoodDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            return View(new User());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if ("admin@food.com".Equals(user.email) && "12345".Equals(user.password))
            {
                HttpContext.Session.SetString("LoggedIN", "True");
                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Home", "Admin");
            }
            else
            {
                List<User> dbusers = dbContext.Users.ToList();
                foreach (User dbuser in dbusers)
                {
                    if (dbuser != null && dbuser.email.Equals(user.email) && dbuser.password.Equals(user.password))
                    {
                        HttpContext.Session.SetString("LoggedIN", "True");
                        HttpContext.Session.SetString("UserRole", "Customer");
                        HttpContext.Session.SetString("UserID", dbuser.id.ToString());
                        return RedirectToAction("Index", "Customer");
                    }
                }
            }
            ViewData["Login"] = "Login Failed! Either username or password is incorrect.";
            HttpContext.Session.SetString("LoggedIN", "False");
            return View();
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user)
        {
            dbContext.Add(user);
            dbContext.SaveChanges();
            return Redirect("Index");
        }
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("LoggedIN", "False");
            HttpContext.Session.SetString("UserRole", "");
            HttpContext.Session.SetString("UserID", "");
            return RedirectToAction("Index", "Home");
        }
    }
}
