using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IPersonRepository
    {
        Person GetPerson(int id);
        IEnumerable<Person> GetAllPersons();
        Person Add(Person person);
        Person Update(Person person);
        Person Delete(int id);
    }
}
