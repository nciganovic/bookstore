using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class PersonController : Controller
    {
        private IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        [Route("Admin/Persons")]
        public ViewResult DisplayAllPersons()
        {
            var model = personRepository.GetAllPersons();
            ViewBag.Title = "All persons";
            return View("Views/Admin/Person/DisplayAllPersons.cshtml", model);
        }

        [HttpGet]
        [Route("Admin/Persons/Create")]
        public ViewResult CreatePerson()
        {
            return View("Views/Admin/Person/CreatePerson.cshtml");
        }

        [HttpPost]
        [Route("Admin/Persons/Create")]
        public IActionResult CreatePerson(Person person)
        {
            if (ModelState.IsValid)
            {
                personRepository.Add(person);
                return RedirectToAction("DisplayAllPersons");
            }

            return View("Views/Admin/Person/CreatePerson.cshtml");
        }
    }
}
