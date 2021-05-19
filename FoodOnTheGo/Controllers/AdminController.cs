using FoodOnTheGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Controllers
{
    public class AdminController : Controller
    {
        private readonly FoodDbContext dbContext;
        public AdminController(FoodDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                List<Orders> Dborders = dbContext.Orders.ToList();
                List<Orders> orders = new List<Orders>();
                foreach (Orders order in Dborders)
                {
                    if (order.Status.Equals("Initializing"))
                    {
                        orders.Add(order);
                    }
                }
                return View(orders);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult ViewOrderItems(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                List<OrderItem> items = dbContext.OrderItems.ToList();
                List<OrderItem> orderitems = new List<OrderItem>();
                foreach (var item in items)
                {
                    if (id == item.OrderID)
                    {
                        item.menu = new Menu();
                        item.menu = dbContext.MenuItems.Find(item.menuID);
                        orderitems.Add(item);
                    }
                }
                return View(orderitems);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Home()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                List<Menu> items = dbContext.MenuItems.ToList();
                List<Menu> item = new List<Menu>();
                foreach (Menu tt in items)
                {
                    if (tt.quantity > 0)
                    {
                        item.Add(tt);
                    }
                }
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult UpdateOrderStatus(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Admin"))
            {
                Orders order = dbContext.Orders.Find(id);
                order.Status = "Prepared & Delivered";
                dbContext.Entry(order).State = EntityState.Modified;
                dbContext.SaveChanges();
                return Redirect("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
