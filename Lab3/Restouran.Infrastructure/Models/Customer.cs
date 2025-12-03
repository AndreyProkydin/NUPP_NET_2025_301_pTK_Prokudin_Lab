using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public float TotalSpent { get; set; }

        public Order? order { get; set; }


        public void CalculateTotalSpent()
        {
            TotalSpent = order?.MenuItem.Sum(m => m.Price) ?? 0;
        }
    }
}
