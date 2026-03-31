using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
