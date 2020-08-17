﻿using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }

        public string Id { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public Person Person { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
    }
}
