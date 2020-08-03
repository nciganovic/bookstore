using BookStore.Models.Dto;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BookStore.Models.SqlRepository
{
    public class SqlAuthorBookRepository : IAuthorBookRepository
    {
        private AppDbContext context;

        public SqlAuthorBookRepository(AppDbContext context) {
            this.context = context;
        }

        public AuthorBook Add(AuthorBook authorBook)
        {
            context.AuthorBooks.Add(authorBook);
            context.SaveChanges();
            return authorBook;
        }

        public AuthorBook Delete(int id)
        {
            AuthorBook AbToDelete = context.AuthorBooks.Find(id);
            if (AbToDelete != null) {
                context.AuthorBooks.Remove(AbToDelete);
                context.SaveChanges();
            }

            return AbToDelete;
        }

        public IEnumerable<GetAuthorBookDto> GetAllAuthorBooks()
        {
            var data = context.AuthorBooks.Select(x => new GetAuthorBookDto {
                Id = x.Id,
                AuthorName = x.Author.Person.FirstName + " " + x.Author.ArtistName + " " + x.Author.Person.LastName,
                BookTitle = x.Book.Title
            }).ToList();

            return data;
        }

        public AuthorBook GetAuthorBook(int id)
        {
            return context.AuthorBooks.Find(id);
        }

        public bool IsAuthorBookUnique(AuthorBook authorBook)
        {
            var getAuthorBookByFks = context.AuthorBooks.Where(x => x.AuthorId == authorBook.AuthorId && x.BookId == authorBook.BookId).ToList();

            if (getAuthorBookByFks.Any())
            {
                return false;
            }
            else {
                return true;
            }
        }

        public AuthorBook Update(AuthorBook authorBook)
        {
            var abToUpdate = context.AuthorBooks.Attach(authorBook);
            abToUpdate.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return authorBook;
        }
    }
}
