using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private IAuthorRepository authorRepository;
        private IPersonRepository personRepository;

        public AuthorController(IAuthorRepository authorRepository, IPersonRepository personRepository) {
            this.authorRepository = authorRepository;
            this.personRepository = personRepository;
        }

        [Route("Admin/Authors")]
        public ViewResult DisplayAllAuthors()
        {
            var model = authorRepository.GetAllAuthors();
            ViewBag.Title = "All authors";
            return View("Views/Admin/Author/DisplayAllAuthors.cshtml", model);
        }

        [HttpGet]
        [Route("Admin/Authors/Create")]
        public IActionResult CreateAuthor()
        {
            AdminCreateAuthorViewModel viewModel = GetRequestCreateAuthor();
            return View("Views/Admin/Author/CreateAuthor.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Authors/Create")]
        public IActionResult CreateAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                authorRepository.Add(author);
                return RedirectToAction("DisplayAllAuthors");
            }

            AdminCreateAuthorViewModel viewModel = GetRequestCreateAuthor();
            return View("Views/Admin/Author/CreateAuthor.cshtml", viewModel);
        }

        public AdminCreateAuthorViewModel GetRequestCreateAuthor()
        {
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

        public List<Person> GetUniquePersons(IEnumerable<Person> personsModel, List<int> UsedPersonIds)
        {
            var personList = personsModel.ToList();
            foreach (var person in personsModel)
            {
                if (UsedPersonIds.Contains(person.Id))
                {
                    personList.Remove(person);
                }
            }

            return personList;
        }

        public List<int> UsedPersonIds()
        {
            List<int> allPersonIds = new List<int>();
            var authors = authorRepository.GetAllAuthors();

            foreach (var author in authors)
            {
                allPersonIds.Add(author.PersonId);
            }

            return allPersonIds;
        }
    }
}
