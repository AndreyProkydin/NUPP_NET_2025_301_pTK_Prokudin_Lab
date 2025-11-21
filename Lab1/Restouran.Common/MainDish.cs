using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Common
{
    public class MainDish : MenuItem
    {
        public string TypeOfDish { get; set; }
        public string Category { get; set;}
        public bool IsSpicy { get; set; }
        
    }
}
