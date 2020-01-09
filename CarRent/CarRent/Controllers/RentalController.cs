﻿using CarRent.Data;
using CarRent.Infrastructure;
using CarRent.Models;
using CarRent.Models.CarModels;
using CarRent.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{
    public class RentalController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly DateSession _dateSession;

        public RentalController(AppDbContext context, UserManager<User> userManager, DateSession dateSession)
        {
            _context = context;
            _userManager = userManager;
            _dateSession = dateSession;
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

            var carInDb = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
            if (carInDb == null)
                return RedirectToAction("Index", "Car");

            var rentalDate = _dateSession.date;
            int numberOfRentDays = (int)(rentalDate.ReturnDate - rentalDate.RentDate).Value.TotalDays;

            var rental = new Rental
            {
                CarId = carInDb.Id,
                UserId = currentUserId,
                TotalPrice = numberOfRentDays * carInDb.RentPrice,
                RentalStatusId = (byte)Data.RentalStatus.Reservation,
                RentDate = rentalDate.RentDate,
                ReturnDate = rentalDate.ReturnDate
            };



            return View("Index","Home");
        }
    }
}
