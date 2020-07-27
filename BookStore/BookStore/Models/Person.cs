using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [MaxLength(20, ErrorMessage = "First name can't be larger then 20 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [MaxLength(20, ErrorMessage = "Last name can't be larger then 20 characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

    }
}
