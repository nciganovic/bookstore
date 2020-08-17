using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Admin/Role/Create")]
        public IActionResult CreateRole()
        {
            return View("Views/Admin/Role/CreateRole.cshtml");
        }

        [HttpPost]
        [Route("Admin/Role/Create")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("DisplayAllRoles", "Role");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }

            return View("Views/Admin/Role/CreateRole.cshtml");
        }

        public IActionResult DisplayAllRoles()
        {
            IEnumerable<IdentityRole> roles = roleManager.Roles.ToList();
            return View("Views/Admin/Role/DisplayAllRoles.cshtml", roles);
        }

        [HttpGet]
        [Route("Admin/Role/Edit/{id}")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            EditRoleViewModel viewModel = new EditRoleViewModel
            {
                Id = id,
                RoleName = role.Name,
            };

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    viewModel.Users.Add(user.UserName);
                }
            }

            return View("Views/Admin/Role/EditRole.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Role/Edit/{id}")]
        public async Task<IActionResult> EditRole(EditRoleViewModel viewModel)
        {
            var role = await roleManager.FindByIdAsync(viewModel.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {viewModel.Id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }
            else
            {
                role.Name = viewModel.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("DisplayAllRoles", "Role");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }

                return View("Views/Admin/Role/EditRole.cshtml", viewModel);
            }
        }

        [HttpGet]
        [Route("Admin/Role/Delete/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            await roleManager.DeleteAsync(role);
            return RedirectToAction("DisplayAllRoles", "Role");
        }

        [HttpGet]
        [Route("Admin/Role/RoleUsers/{id}")]
        public async Task<IActionResult> EditUserInRole(string id)
        {

            ViewBag.RoleId = id;

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            List<UserRoleViewModel> model = new List<UserRoleViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                UserRoleViewModel userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View("Views/Admin/Role/EditRoleUsers.cshtml", model);
        }

        [HttpPost]
        [Route("Admin/Role/RoleUsers/{id}")]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            for (int i = 0; i < model.Count(); i++) {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name))) {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name))) {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded) {
                    if (i < (model.Count - 1)) {
                        continue;
                    }
                    else{
                        return RedirectToAction("EditRole", new { Id = id });
                    }
                }

            }

            return RedirectToAction("EditRole", new { Id = id });
        }
    }
}
