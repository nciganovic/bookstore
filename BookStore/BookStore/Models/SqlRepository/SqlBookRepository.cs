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

        public IEnumerable<GetBookDto> GetNewestBooks(int count)
        {
            var data = context.Books.Select(x => new GetBookDto
            {
                Id = x.Id,
                Amount = x.Amount,
                CategoryName = x.Category.Name,
                DatePublished = x.DatePublished,
                PageCount = x.PageCount,
                Price = x.Price,
                Title = x.Title,
                PhotoName = x.PhotoName
                
            }).Take(count).OrderByDescending(x => x.DatePublished).ToList();

            return data;
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
                Title = x.Title,
                PhotoName = x.PhotoName
            }).ToList();

            return data;
        }

        public Book GetBook(int id)
        {
            return context.Books.Find(id);
        }

        public GetBookDto GetBookDetails(int id) {
            var book = context.Books.Where(x => x.Id == id);

            var data = book.Select(x => new GetBookDto
            {
                Id = x.Id,
                Amount = x.Amount,
                CategoryName = x.Category.Name,
                CategoryId = x.Category.Id,
                DatePublished = x.DatePublished,
                PageCount = x.PageCount,
                Price = x.Price,
                Title = x.Title,
                PhotoName = x.PhotoName, 
                AuthorsFullName = x.AuthorBook.Select(y => y.Author.Person.FirstName + " "  +  y.Author.Person.LastName).ToList()
            }).FirstOrDefault();

            return data;
        }

        public Book Update(Book book)
        {
            var editBook = context.Books.Attach(book);
            editBook.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return book;
        }

        public IEnumerable<GetBookDto> GetBooksByCategory(int categoryId)
        {
            var books = context.Books.Where(x => x.CategoryId == categoryId);

            var data = books.Select(x => new GetBookDto
            {
                Id = x.Id,
                Amount = x.Amount,
                CategoryName = x.Category.Name,
                DatePublished = x.DatePublished,
                PageCount = x.PageCount,
                Price = x.Price,
                Title = x.Title,
                PhotoName = x.PhotoName

            }).OrderByDescending(x => x.DatePublished).ToList();

            return data;
            
            
        }
    }
}
