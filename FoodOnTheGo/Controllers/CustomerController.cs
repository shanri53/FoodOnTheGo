using FoodOnTheGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FoodDbContext dbContext;
        public CustomerController(FoodDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public IActionResult Index()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
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
        public IActionResult ItemDetail(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                Menu item = dbContext.MenuItems.Find(id);
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                String item = "";
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("Cart")))
                {
                    String[] cart = HttpContext.Session.GetString("Cart").Split("-");
                    if (cart != null && cart.Length > 0)
                    {
                        foreach (string data in cart)
                        {
                            String[] dt = data.Split(":");
                            if (!id.ToString().Equals(dt[0]))
                            {
                                item = item + data + "-";
                            }
                        }
                        item = item + id + ":" + quantity;
                    }
                }
                else
                {
                    item = id + ":" + quantity;
                }
                HttpContext.Session.SetString("Cart", item);
                return Redirect("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult RemoveToCart(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                String item = "";
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("Cart")))
                {
                    String[] cart = HttpContext.Session.GetString("Cart").Split("-");
                    if (cart != null && cart.Length > 0)
                    {
                        bool check = false;
                        foreach (string data in cart)
                        {
                            String[] dt = data.Split(":");
                            if (!id.ToString().Equals(dt[0]))
                            {
                                check = true;
                                item = item + data + "-";
                            }
                        }
                        if (check)
                        {
                            item = item.Substring(0, item.Length - 1);
                        }
                    }
                }
                HttpContext.Session.SetString("Cart", item);
                return Redirect("ViewCartDetail");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult ViewCartDetail()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("Cart")))
                {
                    List<OrderItem> items = new List<OrderItem>();
                    String[] cart = HttpContext.Session.GetString("Cart").Split("-");
                    if (cart != null && cart.Length > 0)
                    {
                        foreach (string data in cart)
                        {
                            String[] dt = data.Split(":");
                            Menu menuitem = dbContext.MenuItems.Find(Int32.Parse(dt[0]));
                            OrderItem orderItem = new OrderItem();
                            orderItem.ID = 0;
                            orderItem.menuID = menuitem.id;
                            orderItem.menu = new Menu();
                            orderItem.menu = menuitem;
                            orderItem.Quantity = Int32.Parse(dt[1]);
                            items.Add(orderItem);
                        }
                    }
                    return View(items);
                }
                return Redirect("Index");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("Cart")))
                {
                    return View(getOrder());
                }
                return Redirect("Index");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult CheckOutPaid()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                updateMenuItemQuantity();
                Orders order = getOrder();
                foreach (var item in order.orderItem)
                {
                    dbContext.OrderItems.Add(item);
                }
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("Cart", "");
                return Redirect("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult ViewPastOrder()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
            {
                List<Orders> DBorders = dbContext.Orders.ToList();
                List<Orders> orders = new List<Orders>();

                foreach (Orders order in DBorders)
                {
                    if (order.UserID.Equals(HttpContext.Session.GetString("UserID")))
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
        public IActionResult ViewPastOrderItems(int id)
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIN")) && HttpContext.Session.GetString("LoggedIN").Equals("True") && HttpContext.Session.GetString("UserRole").Equals("Customer"))
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
        private Orders getOrder()
        {
            decimal total = 0;
            List<OrderItem> items = new List<OrderItem>();
            String[] cart = HttpContext.Session.GetString("Cart").Split("-");
            if (cart != null && cart.Length > 0)
            {
                foreach (string data in cart)
                {
                    String[] dt = data.Split(":");
                    Menu menuitem = dbContext.MenuItems.Find(Int32.Parse(dt[0]));
                    OrderItem orderItem = new OrderItem();
                    orderItem.ID = 0;
                    orderItem.menuID = menuitem.id;
                    orderItem.menu = new Menu();
                    orderItem.menu = menuitem;
                    orderItem.Quantity = Int32.Parse(dt[1]);
                    items.Add(orderItem);
                    total = total + ((decimal)(menuitem.price * orderItem.Quantity));
                }
            }
            Orders order = new Orders();
            order.OrderedAT = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            order.orderItem = new List<OrderItem>();
            order.orderItem = items;
            order.Status = "Initializing";
            order.TotalAmount = total;
            order.UserID = HttpContext.Session.GetString("UserID");
            return order;
        }
        private void updateMenuItemQuantity()
        {
            String[] cart = HttpContext.Session.GetString("Cart").Split("-");
            if (cart != null && cart.Length > 0)
            {
                foreach (string data in cart)
                {
                    String[] dt = data.Split(":");
                    Menu menuitem = dbContext.MenuItems.Find(Int32.Parse(dt[0]));
                    menuitem.quantity = menuitem.quantity - Int32.Parse(dt[1]);
                    dbContext.Entry(menuitem).State = EntityState.Modified;
                }
                dbContext.SaveChanges();
            }
        }
    }
}