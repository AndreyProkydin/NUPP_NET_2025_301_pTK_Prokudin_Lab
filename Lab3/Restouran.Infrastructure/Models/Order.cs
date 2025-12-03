using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public string Status { get;set; }
        public DateTime OrderDate { get; set; }

        // One-to-one(child)
        public int CustomerId { get; set; }
        public Customer? customer { get; set; }

        // One-to-many (parent)
        public ICollection<MenuItem> MenuItem { get; } = new List<MenuItem>();
    }
}
