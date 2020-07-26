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
    }
}
