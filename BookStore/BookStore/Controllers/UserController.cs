using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Static;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IPersonRepository personRepository;

        public UserController(UserManager<ApplicationUser> userManager, IPersonRepository personRepository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.personRepository = personRepository;
        }

        [HttpGet]
        [Route("Admin/Users")]
        public async Task<IActionResult> DisplayAllUsers()
        {
            List<UserViewModel> userList = new List<UserViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            { // user.Person for some reason returns null
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

        [HttpGet]
        [Route("Admin/Users/Edit/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }


            var roles = await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);
            Person person = personRepository.GetPerson(user.PersonId);

            EditUserViewModel viewModel = new EditUserViewModel
            {
                Id = id,
                Email = user.Email,
                UserName = user.UserName,
                Person = person,
                Roles = roles,
                Claims = claims.Select(c => c.Type + " : " + c.Value).ToList()
            };

            return View("Views/Admin/User/EditUser.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Users/Edit/{id}")]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    personRepository.Update(model.Person);

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("DisplayAllUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View("Views/Admin/User/EditUser.cshtml", model);
            }
        }

        [HttpPost]
        [Route("Admin/Users/Delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }

            await userManager.DeleteAsync(user);

            return RedirectToAction("DisplayAllUsers");
        }

        [HttpGet]
        [Route("Admin/Users/EditUserRoles/{id}")]
        public async Task<IActionResult> EditUserRoles(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            ViewBag.UserId = user.Id;

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }

            List<UserRoleViewModel> model = new List<UserRoleViewModel>();

            foreach (var role in await roleManager.Roles.ToListAsync())
            {
                UserRoleViewModel viewModel = new UserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    viewModel.IsSelected = true;
                }
                else
                {
                    viewModel.IsSelected = false;
                }

                model.Add(viewModel);
            }

            return View("Views/Admin/User/EditUserRoles.cshtml", model);
        }

        [HttpPost]
        [Route("Admin/Users/EditUserRoles/{id}")]
        public async Task<IActionResult> EditUserRoles(List<UserRoleViewModel> model, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            for (int i = 0; i < model.Count(); i++)
            {
                var role = await roleManager.FindByIdAsync(model[i].RoleId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditUser", new { Id = id });
                    }
                }

            }

            return RedirectToAction("EditUser", new { Id = id });
        }

        [HttpGet]
        [Route("Admin/Users/EditUserClaims/{id}")]
        public async Task<IActionResult> EditUserClaims(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = id
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View("Views/Admin/User/EditUserClaims.cshtml", model);
        }

        [HttpPost]
        [Route("Admin/Users/EditUserClaims/{id}")]
        public async Task<IActionResult> EditUserClaims(string id, UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found.";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);

            foreach (var claim in claims) {
                var result = await userManager.RemoveClaimAsync(user, claim);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove existing user claims");
                    return View("Views/Admin/User/EditUserClaims.cshtml", model);
                }
            }

            foreach (var claim in model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false"))) {
                var addClaim = await userManager.AddClaimAsync(user, claim);

                if (!addClaim.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected user claims");
                    return View("Views/Admin/User/EditUserClaims.cshtml", model);
                }
            }

            return RedirectToAction("EditUser", new { Id = id });
        }
    }
}
