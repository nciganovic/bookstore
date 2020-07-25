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
    }
}
