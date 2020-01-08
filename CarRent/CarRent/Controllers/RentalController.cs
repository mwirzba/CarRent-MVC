using CarRent.Data;
using CarRent.Models.CarModels;
using CarRent.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{
    public class RentalController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public RentalController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
   
        public async Task<IActionResult> New(long carId)
        {
            if(!User.Identity.IsAuthenticated)
            {
                ViewData["Message"] = "To continue order login or register";

                return await Task.Run<IActionResult>(() =>
               {
                   return View("../Account/Login",new LoginViewModel());
               });
            }

            string currentUserId = _userManager.GetUserId(User);


            return View("Index","Home");
        }






    }
}
