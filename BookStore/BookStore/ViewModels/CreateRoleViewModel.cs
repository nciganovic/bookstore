using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only.")]
        public string RoleName { get; set; }
    }
}
