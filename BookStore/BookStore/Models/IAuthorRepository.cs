using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IAuthorRepository
    {
        Author GetAuthor(int id);
        IEnumerable<GetAuthorDto> GetAllAuthors();
        Author Add(Author author);
        Author Update(Author author);
        Author Delete(int id);
    }
}
