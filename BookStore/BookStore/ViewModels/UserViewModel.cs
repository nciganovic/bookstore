using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ApplicationUser User { get; set; }
    }
}
