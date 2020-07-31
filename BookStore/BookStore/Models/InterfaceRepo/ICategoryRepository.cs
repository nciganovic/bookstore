using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface ICategoryRepository
    {
        public Category Add(Category category);
        public Category Update(Category category);
        public Category Delete(int id);
        public Category GetCategory(int id);
        public IEnumerable<Category> GetAllCategories();
    }
}
