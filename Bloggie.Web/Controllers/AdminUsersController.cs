﻿using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller {
        private readonly IUserRepository userRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager) {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List() {
            var user = await userRepository.GetAll();
            var userViewModel = new UserViewModel();
            userViewModel.Users = new List<User>();


            foreach (var item in user) {
                userViewModel.Users.Add(new Models.ViewModels.User {
                    Id = Guid.Parse(item.Id),
                    Username = item.UserName,
                    EmailAddress = item.Email,
                });
            }



            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request) {
            var identityUser = new IdentityUser {
                UserName = request.Username,
                Email = request.Email,
                
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null) {
                if (identityResult.Succeeded) {
                    var roles = new List<string> { "User" };
                    if (request.AdminRoleCheckbox) {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded) {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }
            return View();
        }
    }
}
