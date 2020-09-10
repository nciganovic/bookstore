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
    }
}
