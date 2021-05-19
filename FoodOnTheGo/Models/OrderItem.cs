using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public virtual Orders order { get; set; }
        public int menuID { get; set; }
        public virtual Menu menu { get; set; }
    }
}
