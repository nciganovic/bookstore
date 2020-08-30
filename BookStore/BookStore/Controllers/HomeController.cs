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
        private IBookRepository bookRepository;
        private readonly IDataProtector dataProtector;

        public HomeController(IPersonRepository personRepository, 
                                ILogger<ApplicationUser> logger, 
                                IBookRepository bookRepository,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings) 
        {
            this.personRepository = personRepository;
            this.logger = logger;
            this.bookRepository = bookRepository;
            dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [Route("/")]
        [Route("~/Home")]
        [Route("[action]")]
        public ViewResult Index()
        {
            IEnumerable<GetBookDto> newestBooks = bookRepository.GetNewestBooks(12).Select(x => {
                x.EncryptedId = dataProtector.Protect(x.Id.ToString());
                return x;
            });

            return View(newestBooks);
        }

        
    }
}
