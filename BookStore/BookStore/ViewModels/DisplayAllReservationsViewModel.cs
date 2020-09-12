using BookStore.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class DisplayAllReservationsViewModel
    {
        public List<GetBookUserDto> CurrentReservations;
        public List<GetBookUserDto> PastReservations;
    }
}
