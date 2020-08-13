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
        public IActionResult CreateRole() {
            return View("Views/Admin/Role/CreateRole.cshtml");
        }

        [HttpPost]
        [Route("Admin/Role/Create")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid) {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded) {
                    return RedirectToAction("DisplayAllRoles", "Role");
                }

                foreach (IdentityError error in result.Errors) {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }

            return View("Views/Admin/Role/CreateRole.cshtml");
        }

        public IActionResult DisplayAllRoles() {
            IEnumerable<IdentityRole> roles = roleManager.Roles.ToList();
            return View("Views/Admin/Role/DisplayAllRoles.cshtml", roles);
        }

        [HttpGet]
        [Route("Admin/Role/Edit/{id}")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null) {
                ViewBag.ErrorMessage = $"Role with id {id} is not found.";
                return View("Views/Home/NotFound.cshtml");
            }

            EditRoleViewModel viewModel = new EditRoleViewModel
            {
                Id = id,
                RoleName = role.Name,
            };

            foreach (var user in await userManager.Users.ToListAsync()) {
                if (await userManager.IsInRoleAsync(user, role.Name)) {
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
            else {
                role.Name = viewModel.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded) {
                    return RedirectToAction("DisplayAllRoles", "Role");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }

                return View("Views/Admin/Role/EditRole.cshtml", viewModel);

            }
        }
    }
}
