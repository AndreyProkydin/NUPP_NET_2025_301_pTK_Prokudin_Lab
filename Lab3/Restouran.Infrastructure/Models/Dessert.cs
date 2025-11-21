using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    public class Dessert: MenuItem
    {
        public string TypeOfDessert { get; set; }
        public string TemperatureService { get; set; }
    }
}
