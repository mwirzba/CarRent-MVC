using CarRent.Data;
using CarRent.Models;
using CarRent.Models.Identity;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{
    //[Authorize(Roles = RoleNames.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(
            UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> EditUser(string userId)
        {
            var userToEdit = await _userManager.FindByIdAsync(userId);
            var userDetailViewModel = new UserDetailsViewModel
            {
                Roles  = _roleManager.Roles,
                User = userToEdit,
                UserRoles = await _userManager.GetRolesAsync(userToEdit)
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
            return RedirectToAction("EditUser","Admin",new UserIdViewModel { UserId = newUserRoleViewModel.UserId });
        }

        public ViewResult UserList()
        {
            var users = _userManager.Users;
            return View(users);
        }

    }
}
