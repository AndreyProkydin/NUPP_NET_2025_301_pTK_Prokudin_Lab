using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } 
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();
    }
}
