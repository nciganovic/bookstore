using BookStore.Models.Dto;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.InterfaceRepo
{
    public interface IBookRepository
    {
        public Book GetBook(int id);
        public Book Add(Book book);
        public Book Update(Book book);
        public Book Delete(int id);
        public IEnumerable<GetBookDto> GetAllBooks();
    }
}
