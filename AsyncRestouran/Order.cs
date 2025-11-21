using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncRestouran
{
    public class Order
    {
        private static readonly Random _random = new Random();
        public Guid Id { get; set; }
        public string TypeOrder {  get; set; }
        public DateTime DateOrder { get; set; }
        public float OrderPrice { get; set; }
        public List<String> OrderPosition { get; set; }


        public static Order CreateNew()
        {
            var orderTypes = new[] { "Dine-in", "Takeaway", "Delivery" };
            var menuItems = new[] { "Pizza", "Salad", "Steak", "Burger", "Pasta", "Soup" };

            var positions = new List<string>();
            int itemsCount = _random.Next(2, 4);
            for (int i = 0; i < itemsCount; i++)
            {
                positions.Add(menuItems[_random.Next(menuItems.Length)]);
            }

            return new Order
            {
                TypeOrder = orderTypes[_random.Next(orderTypes.Length)],
                DateOrder = DateTime.Now.AddDays(-_random.Next(0, 30)), 
                OrderPrice = (float)Math.Round(_random.Next(150, 2000) * _random.NextDouble(), 2), 
                OrderPosition = positions
            };
        }
    }
}
