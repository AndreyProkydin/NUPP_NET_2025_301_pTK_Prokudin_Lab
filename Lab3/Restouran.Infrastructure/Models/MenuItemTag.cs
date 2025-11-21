using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    public class MenuItemTag
    {
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
