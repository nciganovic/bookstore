using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BookStore.Models.Tables;

namespace BookStore.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Artist name")]
        public string ArtistName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select item.")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public List<AuthorBook> AuthorBook { get; set; }
    }
}
