using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Common
{
    public class Customer
    {
        private static int _customerConter = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }   
        public List<Order> OrderHistory { get; set; }
        public float TotalSpent { get; set; }

        static Customer()
        {
            _customerConter = 0;
        }

        public Customer()
        {
            OrderHistory = new List<Order>();
            Id = ++_customerConter;
        }

        public Customer(string name, string phone) : this()
        {
            Name = name;
            Phone = phone;
        }


        public void AddOrder(Order order)
        {
            OrderHistory.Add(order);
            TotalSpent += order.GetPrice();

        }
    }
}
