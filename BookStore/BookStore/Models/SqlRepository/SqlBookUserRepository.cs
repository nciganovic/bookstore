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

        public BookUser Find(int bookId, string userId)
        {
            var book = context.BookUser.Where(x => x.BookId == bookId && x.UserId == userId && x.EndDate > DateTime.Now);

            var data = book.Select(x => new BookUser
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                UserId = x.UserId
            }).FirstOrDefault();

            return data;
        }
    }
}
