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
using Microsoft.AspNetCore.DataProtection;
using BookStore.Security;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.Dto;

namespace BookStore.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IPersonRepository personRepository;
        private ILogger<ApplicationUser> logger;
        private IDataProtectionProvider dataProtectionProvider;
        private IBookRepository bookRepository;

        public HomeController(IPersonRepository personRepository, 
                                ILogger<ApplicationUser> logger, 
                                IDataProtectionProvider dataProtectionProvider,
                                IBookRepository bookRepository) 
        {
            this.personRepository = personRepository;
            this.logger = logger;
          
            
            this.bookRepository = bookRepository;
           // protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings)
        }

        [Route("/")]
        [Route("~/Home")]
        [Route("[action]")]
        public ViewResult Index()
        {
            IEnumerable<GetBookDto> newestBooks = bookRepository.GetNewestBooks(12);
            return View(newestBooks);
        }

        
    }
}
