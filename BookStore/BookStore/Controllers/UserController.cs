using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPersonRepository personRepository;

        public UserController(UserManager<ApplicationUser> userManager, IPersonRepository personRepository)
        {
            this.userManager = userManager;
            this.personRepository = personRepository;
        }

        [HttpGet]
        [Route("Admin/Users")]
        public async Task<IActionResult> DisplayAllUsers()
        {
            List<UserViewModel> userList = new List<UserViewModel>();

            foreach(var user in await userManager.Users.ToListAsync()) { // user.Person for some reason returns null
                Person person = personRepository.GetPerson(user.PersonId);

                var viewModel = new UserViewModel
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    User = user,
                };

                userList.Add(viewModel);
            }

            IEnumerable<UserViewModel> users = userList;

            return View("Views/Admin/User/DisplayAllUsers.cshtml", users);
        }
    }
}
