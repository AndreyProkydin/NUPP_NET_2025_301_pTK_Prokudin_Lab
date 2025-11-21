using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure.Models
{
    public class UsersModels : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);
    }
}
