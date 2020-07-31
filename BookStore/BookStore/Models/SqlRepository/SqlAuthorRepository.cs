using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SqlAuthorRepository : IAuthorRepository
    {
        private AppDbContext context;

        public SqlAuthorRepository(AppDbContext context) {
            this.context = context;
        }

        public Author Add(Author author)
        {
            context.Authors.Add(author);
            context.SaveChanges();
            return author;
        }

        public Author Delete(int id)
        {
            Author authorToDelete = context.Authors.Find(id);
            if (authorToDelete != null) {
                context.Authors.Remove(authorToDelete);
                context.SaveChanges();
            }

            return authorToDelete;
        }

        public IEnumerable<GetAuthorDto> GetAllAuthors()
        {
            var data = context.Authors.Select(x => new GetAuthorDto
            {
               Id = x.Id,
               ArtistName = x.ArtistName,
               FullName = x.Person.FirstName + " " + x.Person.LastName,
               PersonId = x.PersonId
            }).ToList();

            return data;
        }

        public Author GetAuthor(int id)
        {
            return context.Authors.Find(id);
        }

        public Author Update(Author author)
        {
            var updatedAuthor = context.Authors.Attach(author);
            updatedAuthor.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return author;
        }
    }
}
