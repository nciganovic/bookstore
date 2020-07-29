using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class AdminCreateAuthorViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Person> AllPersons { get; set; }
    }
}
