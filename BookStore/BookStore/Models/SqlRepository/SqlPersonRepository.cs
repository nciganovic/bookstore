using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SqlPersonRepository : IPersonRepository
    {
        private AppDbContext context;
        
        public SqlPersonRepository(AppDbContext context) {
            this.context = context;
        }

        public Person GetPerson(int id)
        {
            Person personToGet = context.Persons.Find(id);
            return personToGet;
        }

        public IEnumerable<Person> GetAllPersons()
        {
            var data = context.Persons.Select(x => new Person
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();

            return data;
        }

        public Person Add(Person person)
        {
            context.Persons.Add(person);
            context.SaveChanges();
            return person;
        }

        public Person Update(Person person)
        {
            var editPerson = context.Persons.Attach(person);
            editPerson.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return person;
        }

        public Person Delete(int id)
        {
            Person personToDelete = context.Persons.Find(id);
            if (personToDelete != null) {
                context.Persons.Remove(personToDelete);
                context.SaveChanges();
            }

            return personToDelete;
        }   
    }
}
