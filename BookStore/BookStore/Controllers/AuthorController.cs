using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

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
            CreateAuthorViewModel viewModel = GetAuthorViewModel(new Author(), null);
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

            CreateAuthorViewModel viewModel = GetAuthorViewModel(new Author(), null);
            return View("Views/Admin/Author/CreateAuthor.cshtml", viewModel);
        }

        public CreateAuthorViewModel GetAuthorViewModel(Author author, int? editPersonId)
        {
            var allPersons = personRepository.GetAllPersons();
            List<int> usedPersonsIds = UsedPersonIds();
            if (editPersonId != null) {
                usedPersonsIds.Remove((int)editPersonId);
            }
            
            List<Person> availablePersons = GetAvailablePersons(allPersons, usedPersonsIds);

            CreateAuthorViewModel viewModel = new CreateAuthorViewModel
            {
                AllPersons = availablePersons,
                Author = author
            };
            return viewModel;
        }

        public List<Person> GetAvailablePersons(IEnumerable<Person> AllPersons, List<int> UsedPersonIds)
        {
            var personList = AllPersons.ToList();
            foreach (var person in AllPersons)
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
        
        [HttpGet]
        [Route("Admin/Authors/Edit/{id}")]
        public IActionResult EditAuthor(int id) {
            Author CurrentAuthor = authorRepository.GetAuthor(id);
            CreateAuthorViewModel viewModel = GetAuthorViewModel(CurrentAuthor, CurrentAuthor.PersonId);
            return View("Views/Admin/Author/EditAuthor.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Authors/Edit/{id}")]
        public IActionResult EditAuthor(Author author)
        {
            if (ModelState.IsValid) {
                authorRepository.Update(author);
                return RedirectToAction("DisplayAllAuthors");
            }

            Author CurrentAuthor = authorRepository.GetAuthor(author.Id);
            CreateAuthorViewModel viewModel = GetAuthorViewModel(CurrentAuthor, CurrentAuthor.PersonId);
            return View("Views/Admin/Author/EditAuthor.cshtml", viewModel);
        }
    }
}
