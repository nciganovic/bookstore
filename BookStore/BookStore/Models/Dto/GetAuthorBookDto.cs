using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Dto
{
    public class GetAuthorBookDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
    }
}
