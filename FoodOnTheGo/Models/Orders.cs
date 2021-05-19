using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOnTheGo.Models
{
    public class Orders
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime OrderedAT { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<OrderItem> orderItem { get; set; }
    }
}
