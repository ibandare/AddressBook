using AddressBook.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBook.Service
{
    public interface IEntryService<T>
    {
        Task AddAll(IEnumerable<T> data);
        void DeleteAll();
        Task<List<Entry>> FindAll(int skip = 0, int take = 100);
    }
}
