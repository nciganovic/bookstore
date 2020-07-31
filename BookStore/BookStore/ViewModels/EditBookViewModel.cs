using BookStore.Models;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class EditBookViewModel
    {
        public IEnumerable<Category> AllCategories { get; set; }
        public Book Book { get; set; }
    }
}
