using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonRepository personRepository;

        public HomeController(IPersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        public string Index()
        {
            return personRepository.GetPerson(1).FirstName;
        }

        public ViewResult Details(int id)
        {
            Person model = this.personRepository.GetPerson(id);
            return View();
        }
    }
}
