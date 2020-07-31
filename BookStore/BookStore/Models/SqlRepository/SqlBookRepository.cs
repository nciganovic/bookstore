using BookStore.Models.Dto;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.SqlRepository
{
    public class SqlBookRepository : IBookRepository
    {
        private AppDbContext context;

        public SqlBookRepository(AppDbContext context) {
            this.context = context;
        }

        public Book Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return book;
        }

        public Book Delete(int id)
        {
            Book bookToDelete = context.Books.Find(id);
            if (bookToDelete != null) {
                context.Books.Remove(bookToDelete);
                context.SaveChanges();
            }

            return bookToDelete;
        }

        public IEnumerable<GetBookDto> GetAllBooks()
        {
            var data = context.Books.Select(x => new GetBookDto
            {
                Id = x.Id,
                Amount = x.Amount,
                CategoryName = x.Category.Name,
                DatePublished = x.DatePublished,
                PageCount = x.PageCount,
                Price = x.Price,
                Title = x.Title
            }).ToList();

            return data;
        }

        public Book GetBook(int id)
        {
            return context.Books.Find(id);
        }

        public Book Update(Book book)
        {
            var editBook = context.Books.Attach(book);
            editBook.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return book;
        }
    }
}
