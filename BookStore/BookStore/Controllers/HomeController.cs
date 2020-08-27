using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;
using NLog.Fluent;
using Microsoft.Extensions.Logging;
using BookStore.Models.Tables;

namespace BookStore.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IPersonRepository personRepository;
        private ILogger<ApplicationUser> logger;

        public HomeController(IPersonRepository personRepository, ILogger<ApplicationUser> logger) {
            this.personRepository = personRepository;
            this.logger = logger;
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
