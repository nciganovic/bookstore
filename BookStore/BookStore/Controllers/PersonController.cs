using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route("Admin/Persons/Edit/{id}")]
        public ViewResult EditPerson(int id)
        {
            Person person = personRepository.GetPerson(id);
            if (person == null) {
                ViewBag.Object = "Person";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            return View("Views/Admin/Person/EditPerson.cshtml", person);
        }

        [HttpPost]
        [Route("Admin/Persons/Edit/{id}")]
        public IActionResult EditPerson(Person person)
        {
            if (person == null)
            {
                ViewBag.Object = "Person";
                return View("Views/Home/ObjectNotFound.cshtml", person.Id);
            }

            if (ModelState.IsValid)
            {
                personRepository.Update(person);
                return RedirectToAction("DisplayAllPersons");
            }

            return View("Views/Admin/Person/EditPerson.cshtml", person);
        }

        [HttpGet]
        [Route("Admin/Persons/Delete/{id}")]
        public IActionResult DeletePerson(int id)
        {
            Person person = personRepository.GetPerson(id);
            if (person == null)
            {
                ViewBag.Object = "Person";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            personRepository.Delete(id);
            return RedirectToAction("DisplayAllPersons");
        }
    }
}
