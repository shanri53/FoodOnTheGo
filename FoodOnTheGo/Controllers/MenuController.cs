using FoodOnTheGo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FoodOnTheGo.Controllers
{
    public class MenuController : Controller
    {
        private readonly FoodDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public MenuController(FoodDbContext _dbContext, IWebHostEnvironment hostEnvironment)
        {
            dbContext = _dbContext;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                List<Menu> items = dbContext.MenuItems.ToList();
                return View(items);
            }
            else {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                return View(dbContext.MenuItems.Find(id));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Create(Menu menu)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                string webRoothPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var name = string.Format(@"{0}", DateTime.Now.Ticks);
                if (files.Count != 0)
                {
                    var Uploads = Path.Combine(webRoothPath, @"images");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(Uploads, name + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    menu.Photo = @"\" + @"images" + @"\" + name + extension;
                }
                else
                {
                    var uploads = Path.Combine(webRoothPath, @"images" + @"\" + "default_image.png");
                    System.IO.File.Copy(uploads, webRoothPath + @"\" + @"images" + @"\" + name + ".png");
                    menu.Photo = @"\" + @"images" + @"\" + name + ".png";
                }
                dbContext.Add(menu);
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                return View(dbContext.MenuItems.Find(id));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Edit(int id, Menu menu)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                var menuFromDb = dbContext.MenuItems.Find(id);
                string webRoothPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var name = string.Format(@"{0}", DateTime.Now.Ticks);
                if (files.Count != 0)
                {
                    var Uploads = Path.Combine(webRoothPath, @"images");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(Uploads, name + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    menu.Photo = @"\" + @"images" + @"\" + name + extension;
                }
                else
                {
                    menu.Photo = menuFromDb.Photo;
                }
                dbContext.Entry(menu).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                return View(dbContext.MenuItems.Find(id));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Delete(int id, Menu menu)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                dbContext.MenuItems.Remove(menu);
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
