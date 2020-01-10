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
            UserDetailsViewModel userDetailViewModel = await GetUserDetailsViewModel(userToEdit, userRoles);
            return View("UserDetails", userDetailViewModel);
        }

        public async Task<IActionResult> AddRoleToUser(NewUserRoleViewModel newUserRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(newUserRoleViewModel.UserId);   
            var role = await _roleManager.FindByNameAsync(newUserRoleViewModel.RoleName);

            if (role == null)
            {
                ViewData["Message"] = "Wrong role name!";
                return View("Error");
            }

            var roleResult = await _userManager.AddToRoleAsync(user,role.Name);

            if(!roleResult.Succeeded)
            {
                ViewData["Message"] = "Something got wrong and could not add user to role";
                return View("Error");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userDetailViewModel = GetUserDetailsViewModel(user, userRoles);
            if (userDetailViewModel != null)
                return View("UserDetails", userDetailViewModel);

            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> RemoveUserFromRole(NewUserRoleViewModel newUserRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(newUserRoleViewModel.UserId);
            var role = await _roleManager.FindByNameAsync(newUserRoleViewModel.RoleName);


            if (role == null)
            {
                ViewData["Message"] = "Wrong role name!";
                return View("Error");
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Contains(role.Name) || userRoles == null || role ==null)
            {
                ViewData["Message"] = "Wrong role name!";
                return View("Error");
            }
            

            IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!identityResult.Succeeded)
            {
                ViewData["Message"] = "Something got wrong and could not remove user from role";
                return View("Error");
            }

            userRoles.Remove(role.Name);

            var userDetailViewModel = GetUserDetailsViewModel(user,userRoles);

            if (userDetailViewModel != null)
                return View("UserDetails", userDetailViewModel);

            return RedirectToAction("UserList");
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


        private async Task<UserDetailsViewModel> GetUserDetailsViewModel(User userToEdit,IEnumerable<string> userRoles)
        {
            var userDetailViewModel = new UserDetailsViewModel
            {

                UserRoles = await _userManager.GetRolesAsync(userToEdit),
                Roles = _roleManager.Roles.Where(r => !userRoles.Any(rl => rl == r.Name)),
                User = userToEdit,
            };
            return userDetailViewModel;
        }

    }
}
