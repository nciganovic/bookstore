using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class AuthorBookViewModel
    {
        public IEnumerable<GetAuthorDto> AllAuthors { get; set; }
        public IEnumerable<GetBookDto> AllBooks { get; set; }
        public AuthorBook AuthorBook { get; set; }
    }
}
