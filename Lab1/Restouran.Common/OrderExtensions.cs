using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Common
{
    public static class OrderExtensions
    {
        public static IEnumerable<MainDish> GetMainDishes(this Order order)
        {
            return order.Items.OfType<MainDish>();
        }

        public static IEnumerable<Dessert> GetDesserts(this Order order)
        {
            return order.Items.OfType<Dessert>();
        }
    }
}
