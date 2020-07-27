using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class MockPersonRepository : IPersonRepository
    {
        private List<Person> personList = new List<Person>();

        public MockPersonRepository() {
            Person p1 = new Person { Id = 1, FirstName = "Pera", LastName = "Peric" };
            Person p2 = new Person { Id = 2, FirstName = "Mika", LastName = "Mikic" };
            Person p3 = new Person { Id = 3, FirstName = "Laza", LastName = "Lazic" };

            personList.Add(p1);
            personList.Add(p2);
            personList.Add(p3);
        }

        public Person GetPerson(int id) {
            return personList.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Person> GetAllPersons() {
            return personList;
        }

        public Person Add(Person person)
        {
            person.Id = personList.Max(x => x.Id) + 1;
            personList.Add(person);
            return person;
        }

        public Person Update(Person person)
        {
            Person personToUpdate = personList.FirstOrDefault(x => x.Id == person.Id);
            if (personToUpdate != null)
            {
                personToUpdate.FirstName = person.FirstName;
                personToUpdate.LastName = person.LastName;
            }

            return personToUpdate;
        }

        public Person Delete(int id)
        {
            var personToDelete = personList.FirstOrDefault(x => x.Id == id);
            if (personToDelete != null) {
                personList.Remove(personToDelete);
            }
            
            return personToDelete;
        }
    }
}
