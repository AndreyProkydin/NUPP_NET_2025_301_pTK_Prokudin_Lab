using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Models
{
    public class MenuItemModel
    {
        public int IdItem { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public List<TagModel> Tags { get; set; } = new List<TagModel>();
    }
}
