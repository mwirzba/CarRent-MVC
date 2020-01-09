using CarRent.Data;
using CarRent.Models;
using CarRent.Models.Identity;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        public AdminController(
            UserManager<User> userManager,RoleManager<IdentityRole> roleManager,AppDbContext appDbContext) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = appDbContext;
        }
        public async Task<IActionResult> EditUser(string userId)
        {
            var userToEdit = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(userToEdit);
            var userDetailViewModel = new UserDetailsViewModel
            {

                UserRoles = userRoles,
                Roles = _roleManager.Roles.Where(r=> !userRoles.Any(rl => rl == r.Name)),
                User = userToEdit,
            };
            return View("UserDetails", userDetailViewModel);
        }

        public async Task<IActionResult> AddRoleToUser(NewUserRoleViewModel newUserRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(newUserRoleViewModel.UserId);   
            var role = await _roleManager.FindByNameAsync(newUserRoleViewModel.RoleName);

            var userRoles = await _userManager.GetRolesAsync(user);

            if(userRoles.Contains(role.Name) && userRoles!=null)
            {
                //ModelState.AddModelError("")
            }

            var roleResult = await _userManager.AddToRoleAsync(user,role.Name);

            if(!roleResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to build user and roles");
            }
            return RedirectToAction("UserDetails","Admin",new UserIdViewModel { UserId = newUserRoleViewModel.UserId });
        }


        public IActionResult RentalsList()
        {
            var rentals = _dbContext.Rentals
                .Include(u => u.User)
                .Include(c => c.Car)
                .Include(rs => rs.RentalStatus);

            
            return View("RentalsList",rentals);
        }

        public async Task<IActionResult> EditRentalStatus(long id,byte rentalStatusId)
        {
            var rentalToEdit = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            rentalToEdit.RentalStatusId = rentalStatusId;
            if(rentalToEdit!=null)
            {
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(RentalsList));
            }
            return RedirectToAction(nameof(RentalsList));
        }

        public ViewResult UserList()
        {
            var users = _userManager.Users;
            return View(users);
        }

    }
}
