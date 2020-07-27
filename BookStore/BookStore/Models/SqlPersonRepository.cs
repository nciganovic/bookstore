using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SqlPersonRepository : IPersonRepository
    {
        private AppDbContext context;
        // TODO Finish all implementations
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
            throw new NotImplementedException();
        }

        public Person Add(Person person)
        {
            context.Persons.Add(person);
            context.SaveChanges();
            return person;
        }

        public Person Update(Person person)
        {
            throw new NotImplementedException();
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
