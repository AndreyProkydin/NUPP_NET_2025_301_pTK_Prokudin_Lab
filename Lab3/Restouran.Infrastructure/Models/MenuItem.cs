using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    internal class MenuItem
    {
        public int IdItem { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }


        // one-to-many (child)
        public int OrderId { get; set; }
        public Order? order { get; set; }


        public ICollection<MenuItemTag> MenuItemTags { get; set; } = new List<MenuItemTag>();
    }
}
