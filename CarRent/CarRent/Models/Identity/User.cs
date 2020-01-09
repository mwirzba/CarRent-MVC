using Microsoft.AspNetCore.Identity;

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
