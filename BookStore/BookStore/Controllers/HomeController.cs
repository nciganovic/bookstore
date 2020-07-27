using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IPersonRepository personRepository;

        public HomeController(IPersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        [Route("/")]
        [Route("~/Home")]
        [Route("[action]")]
        public ViewResult Index()
        {
            ViewBag.Title = "Welcome to Bookstore";
            return View();
        }

        
    }
}
