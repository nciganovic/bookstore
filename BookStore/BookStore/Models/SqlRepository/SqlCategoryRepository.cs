using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SqlCategoryRepository : ICategoryRepository
    {
        private AppDbContext context;

        public SqlCategoryRepository(AppDbContext context) {
            this.context = context;
        }

        public Category Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category Delete(int id)
        {
            var ctgToDelete = context.Categories.Find(id);

            if (ctgToDelete != null) {
                context.Categories.Remove(ctgToDelete);
                context.SaveChanges();
            }

            return ctgToDelete;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var data = context.Categories.Select(x => new Category {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return data;
        }

        public Category GetCategory(int id)
        {
            return context.Categories.Find(id);
        }

        public Category Update(Category category)
        {
            var editPerson = context.Categories.Attach(category);
            editPerson.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return category;
        }
    }
}
