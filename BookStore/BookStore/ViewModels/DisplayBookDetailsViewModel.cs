using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class DisplayBookDetailsViewModel
    {
        public GetBookDto Book { get; set; }
        public Category Category { get; set; }
    }
}
