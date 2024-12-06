﻿using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace Bloggie.Web.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Return() {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) {
            var identityUser = new IdentityUser {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
                PasswordHash = registerViewModel.Password,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded) {
                //asing this user the User role
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                if (roleIdentityResult.Succeeded) { 
                    //show success notification
                    return RedirectToAction("Register");
                }
            };

            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl) {
            var model = new LoginViewModel {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {

            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if (signInResult != null && signInResult.Succeeded) {

                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) { 
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            //Show Errors
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() {
            return View();
        }

     
    }
}
