using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class GetAuthorDto
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; }
    }
}
