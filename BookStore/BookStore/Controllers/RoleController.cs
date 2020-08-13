using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookStore.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
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
                    return RedirectToAction("DisplayAllRoles", "Role"); //Change to DisplayAllRoles
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
    }
}
