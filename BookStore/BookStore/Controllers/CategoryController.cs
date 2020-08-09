using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult EditCategory(int id) {
            Category category = categoryRepository.GetCategory(id);
            if (category == null)
            {
                ViewBag.Object = "Category";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            return View("Views/Admin/Category/EditCategory.cshtml", category);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult EditCategory(Category category)
        {
            if (category == null)
            {
                ViewBag.Object = "Category";
                return View("Views/Home/ObjectNotFound.cshtml", category.Id);
            }

            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                return RedirectToAction("DisplayAllCategories");
            }

            return View("Views/Admin/Category/EditCategory.cshtml", category);
        }
        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult DeleteCategory(int id) {
            Category category = categoryRepository.GetCategory(id);
            if (category == null)
            {
                ViewBag.Object = "Category";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }


            categoryRepository.Delete(((Category)category).Id);
            return RedirectToAction("DisplayAllCategories");
        }

      

    }
}
