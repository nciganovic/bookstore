using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Tables;

namespace BookStore.ViewModels
{
    public class CreateBookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Category> AllCategories { get; set; } 
    }
}
