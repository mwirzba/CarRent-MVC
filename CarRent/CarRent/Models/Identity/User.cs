using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.Identity
{
    public class User : IdentityUser
    {
        [ProtectedPersonalData]
        public string Name { get; set; }

        [ProtectedPersonalData]
        public string  SurName { get; set; }
    }
    
}
