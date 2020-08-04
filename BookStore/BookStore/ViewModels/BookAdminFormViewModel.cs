using BookStore.Models;
using BookStore.Models.Tables;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookAdminFormViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Category> AllCategories { get; set; }
        public IFormFile Photo { get; set; }        
    }
}
