using AddressBook.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Service.Impl
{
    public class EntryService : IEntryService<Entry>
    {
        private readonly ApplicationContext _context;

        public EntryService(ApplicationContext context)
        {
            _context = context;
        }

        public void DeleteAll()
        {
            _context.Entries.ExecuteDelete();
        }

        public async Task AddAll(IEnumerable<Entry> data)
        {
            await _context.Entries.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }

        public Task<List<Entry>> FindAll(Entry predicate, int skip = 0, int take = 100)
        {
            var query = _context.Entries
                .OrderBy(e => e.Id)
                .Skip(skip)
                .Take(take);


            if (!string.IsNullOrEmpty(predicate.Date))
            {
                query = query.Where(entry => entry.Date.Contains(predicate.Date));
            }
            if (!string.IsNullOrEmpty(predicate.FirstName))
            {
                query = query.Where(entry => entry.FirstName.Contains(predicate.FirstName));
            }
            if (!string.IsNullOrEmpty(predicate.LastName))
            {
                query = query.Where(entry => entry.LastName.Contains(predicate.LastName));
            }
            if (!string.IsNullOrEmpty(predicate.MiddleName))
            {
                query = query.Where(entry => entry.MiddleName.Contains(predicate.MiddleName));
            }
            if (!string.IsNullOrEmpty(predicate.City))
            {
                query = query.Where(entry => entry.City.Contains(predicate.City));
            }
            if (!string.IsNullOrEmpty(predicate.MiddleName))
            {
                query = query.Where(entry => entry.Country.Contains(predicate.Country));
            }

            return query.ToListAsync();
        }
    }
}
