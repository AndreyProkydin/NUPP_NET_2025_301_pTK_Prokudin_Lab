using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Models
{
    public class MenuItemRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
