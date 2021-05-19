using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Models
{
    public class Menu
    {
        public int id { get; set; }
        [Required(ErrorMessage = "The item name is required")]
        [Display(Name = "Item Name")]
        public string itemname { get; set; }
        [Required(ErrorMessage = "The item description is required")]
        public string description { get; set; }
        [Required(ErrorMessage = "The item Quantity is required")]
        [Range(10, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int quantity { get; set; }
        [Required(ErrorMessage = "The item Price is required")]
        [Range(10, 100000,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double price { get; set; }

        [Required(ErrorMessage = "The image is required")]
        public string Photo { get; set; }
    }
}
