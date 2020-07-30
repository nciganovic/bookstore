using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        [Route("")]
        [Route("Home")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
