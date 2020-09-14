using BookStore.Models;
using BookStore.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<GetBookDto> NewestBooks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
