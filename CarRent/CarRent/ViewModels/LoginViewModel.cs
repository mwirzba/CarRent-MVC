using System.ComponentModel.DataAnnotations;

namespace CarRent.Controllers
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name  = "Login or Email")]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}