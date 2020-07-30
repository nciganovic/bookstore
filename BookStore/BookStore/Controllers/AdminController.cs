using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        protected IPersonRepository personRepository;
        protected IAuthorRepository authorRepository;
        protected ICategoryRepository categoryRepository;

        public AdminController(IPersonRepository personRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository) {
            this.personRepository = personRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
        }

        [Route("")]
        [Route("Home")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        /* Person Controller */

        [Route("Persons")]
        public ViewResult DisplayAllPersons()
        {
            var model = personRepository.GetAllPersons();
            ViewBag.Title = "All persons";
            return View(model);
        }

        [HttpGet]
        [Route("Persons/Create")]
        public ViewResult CreatePerson() {

            return View();
        }

        [HttpPost]
        [Route("Persons/Create")]
        public IActionResult CreatePerson(Person person)
        {
            if (ModelState.IsValid) {
                Person newPerson = personRepository.Add(person);
                return RedirectToAction("DisplayAllPersons");
            }

            return View();
        }

        /* Author Controller */

        [Route("Authors")]
        public ViewResult DisplayAllAuthors() {
            var model = authorRepository.GetAllAuthors();
            ViewBag.Title = "All authors";
            return View(model);
        }

        [HttpGet]
        [Route("Authors/Create")]
        public IActionResult CreateAuthor() {
            AdminCreateAuthorViewModel viewModel = GetRequestCreateAuthor();
            return View(viewModel);
        }

        [HttpPost]
        [Route("Authors/Create")]
        public IActionResult CreateAuthor(Author author) {
            if (ModelState.IsValid) {
                authorRepository.Add(author);
                return RedirectToAction("DisplayAllAuthors");
            }

            AdminCreateAuthorViewModel viewModel = GetRequestCreateAuthor();
            return View(viewModel);
        }

        public AdminCreateAuthorViewModel GetRequestCreateAuthor() {
            var personsModel = personRepository.GetAllPersons();
            List<int> usedIds = UsedPersonIds();
            List<Person> availablePersons = GetUniquePersons(personsModel, usedIds);

            AdminCreateAuthorViewModel viewModel = new AdminCreateAuthorViewModel
            {
                AllPersons = availablePersons,
                Author = new Author()
            };
            return viewModel;
        }

        public List<Person> GetUniquePersons(IEnumerable<Person> personsModel, List<int> UsedPersonIds) {
            var personList = personsModel.ToList();           
            foreach (var person in personsModel) {
                if (UsedPersonIds.Contains(person.Id)) {
                    personList.Remove(person);
                }
            }

            return personList;
        }

        public List<int> UsedPersonIds() {
            List<int> allPersonIds = new List<int>();
            var authors = authorRepository.GetAllAuthors();

            foreach (var author in authors) {
                allPersonIds.Add(author.PersonId);
            }

            return allPersonIds;
        }

        /* Categories Controller */
        [Route("Categories")]
        public ViewResult DisplayAllCategories() {
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();
            ViewBag.Title = "All categories";
            return View(allCategories);
        }

        [HttpGet]
        [Route("Categories/Create")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("Categories/Create")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid) {
                categoryRepository.Add(category);
                return RedirectToAction("DisplayAllCategories");
            }

            return View();
        }

    }
}
