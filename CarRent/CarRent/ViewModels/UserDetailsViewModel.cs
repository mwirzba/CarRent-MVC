using CarRent.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }

    }
}
