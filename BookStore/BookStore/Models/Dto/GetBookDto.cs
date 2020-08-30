using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Dto
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PageCount { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime DatePublished { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string PhotoName { get; set; }
        public List<string> AuthorsFullName { get; set; } 
        public string EncryptedId { get; set; }
    }
}
