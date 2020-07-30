using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("Admin/Categories")]
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository) {
            this.categoryRepository = categoryRepository;
        }

        [Route("")]
        public ViewResult DisplayAllCategories()
        {
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();
            ViewBag.Title = "All categories";
            return View("Views/Admin/Category/DisplayAllCategories.cshtml", allCategories);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateCategory()
        {
            return View("Views/Admin/Category/CreateCategory.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                return RedirectToAction("DisplayAllCategories");
            }

            return View("Views/Admin/Category/CreateCategory.cshtml");
        }
    }
}
