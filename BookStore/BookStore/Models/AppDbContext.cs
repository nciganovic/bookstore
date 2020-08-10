using BookStore.Models.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
            
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>()
                .HasKey(t => new { t.Id });

            modelBuilder.Entity<AuthorBook>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.AuthorBook)
                .HasForeignKey(pt => pt.BookId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne(pt => pt.Author)
                .WithMany(t => t.AuthorBook)
                .HasForeignKey(pt => pt.AuthorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
