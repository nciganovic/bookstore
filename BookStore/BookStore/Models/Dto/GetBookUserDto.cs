using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Dto
{
    public class GetBookUserDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }
        public string BookName { get; set; }
     
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
