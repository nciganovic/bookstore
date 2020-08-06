using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode) {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, this page does not exist.";
                    break;
            }

            return View("Views/Home/NotFound.cshtml");
        }
    }
}
