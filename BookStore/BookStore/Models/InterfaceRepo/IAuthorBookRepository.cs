using BookStore.Models.Dto;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.InterfaceRepo
{
    public interface IAuthorBookRepository
    {
        public AuthorBook Add(AuthorBook authorBook);
        public AuthorBook Update(AuthorBook authorBook);
        public IEnumerable<GetAuthorBookDto> GetAllAuthorBooks();
        public AuthorBook Delete(int id);
        public AuthorBook GetAuthorBook(int id);
    }
}
