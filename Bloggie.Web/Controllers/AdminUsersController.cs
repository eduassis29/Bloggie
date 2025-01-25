using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller {
        private readonly IUserRepository userRepository;

        public AdminUsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

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
    }
}
