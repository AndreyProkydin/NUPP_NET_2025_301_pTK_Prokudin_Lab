using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    internal class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MenuItemTag> MenuItemTags { get; set; } = new List<MenuItemTag>();
    }

}

