using BookStore.Models.InterfaceRepo;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.SqlRepository
{
    public class SqlBookUserRepository : IBookUserRepository
    {
        private AppDbContext context;

        public SqlBookUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public BookUser Add(BookUser bookUser)
        {
            context.BookUser.Add(bookUser);
            context.SaveChanges();
            return bookUser;
        }
    }
}
