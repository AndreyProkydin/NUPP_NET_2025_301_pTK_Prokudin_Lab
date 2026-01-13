using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Restouran.Common.MenuItem;

namespace Restouran.Common
{
    public class Order
    {
        private static int _counterOrders = 0;
        public event OrderEventHandler OnStatusChanged;
        public int Id { get; set; }

        public int IdCustomer { get; set; }

        public List<MenuItem>? Items { get; set; }
        private string _status { get; set; }
        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusChanged?.Invoke(this, value);
                }
            }
        }
        public DateTime OrderDate { get; set; }


        public Order()
        {
            Items = new List<MenuItem>();
            OrderDate = DateTime.Now;
            Status = "Нове";
            _counterOrders++;
        }

       public float GetPrice()
       {
            return Items.Sum(item => item.Price);
       }

        public static int GetCountersOrders()
        {
            return _counterOrders;
        }

    }
}
