using CarRent.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarRent.Models.Identity
{
    public class IdentityInitializer
    {
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<User> _userMgr;

        public IdentityInitializer(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("TestAdminUser");

            if (user == null)
            {
                if (!await _roleMgr.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await _roleMgr.CreateAsync(adminRole);
                   //await _roleMgr.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.view"));
                   //await _roleMgr.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.create"));
                   //await _roleMgr.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.update"));

                }

                user = new User()
                {
                    UserName = "TestAdminUser",
                    Name = "Admin",
                    SurName = "User",
                    Email = "test@admin.com"
                };

                var userResult = await _userMgr.CreateAsync(user, "SecretPassword!2");
             
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                //var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded) //|| !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }

    }
}
