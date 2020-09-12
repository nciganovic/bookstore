using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models.Dto;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog.Web.LayoutRenderers;

namespace BookStore.Controllers
{
    public class BookUserController : Controller
    {
        private IBookUserRepository bookUserRepository;
        private UserManager<ApplicationUser> userManager;
        public BookUserController(IBookUserRepository bookUserRepository, UserManager<ApplicationUser> userManager) {
            this.bookUserRepository = bookUserRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Reservations/All/{username}")]
        public async Task<IActionResult> DisplayAllReservations(string username)
        {
            List<GetBookUserDto> currentReservationList = new List<GetBookUserDto>();
            List<GetBookUserDto> pastReservationList = new List<GetBookUserDto>();

            var user = await userManager.FindByNameAsync(username);
            IEnumerable<GetBookUserDto> userReservations = bookUserRepository.Find(user.Id);

            foreach (var reservation in userReservations) {
                if (reservation.EndDate > DateTime.Now)
                {
                    currentReservationList.Add(reservation);
                }
                else {
                    pastReservationList.Add(reservation);
                }
            }

            DisplayAllReservationsViewModel viewModel = new DisplayAllReservationsViewModel { 
                CurrentReservations = currentReservationList,
                PastReservations = pastReservationList
            };

            return View("Views/BookUser/DisplayAllReservations.cshtml", viewModel);
        }
    }
}
