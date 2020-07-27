using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        protected IPersonRepository _personRepository;

        public AdminController(IPersonRepository personRepository) {
            _personRepository = personRepository;
        }

        [Route("")]
        [Route("Home")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Persons")]
        public ViewResult DisplayAllPersons()
        {
            var model = _personRepository.GetAllPersons();
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
                Person newPerson = _personRepository.Add(person);
                return RedirectToAction("DisplayAllPersons");
            }

            return View();
        }

    }
}
