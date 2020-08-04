using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Tables
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Minimum number must be 0")]
        public int PageCount { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Minimum number must be 0")]
        public int Amount { get; set; }
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Minimum number must be 0")]
        public decimal Price { get; set; }
        public DateTime DatePublished { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select item.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<AuthorBook> AuthorBook { get; set; }
        public string PhotoPath { get; set; }
    }
}
