using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Models
{
    public class OrderRequestModel
    {
        public int CustomerId { get; set; }
        public string Status { get; set; }

        public List<int> MenuItemIds { get; set; } = new List<int>();
    }
}
