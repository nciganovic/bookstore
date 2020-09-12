using BookStore.Models.Dto;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.InterfaceRepo
{
    public interface IBookUserRepository
    {
        public BookUser Add(BookUser bookUser);
        public BookUser Find(int bookId, string userId);
        public IEnumerable<GetBookUserDto> Find(string userId);
    }
}
