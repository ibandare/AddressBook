using AddressBook.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Task<List<Entry>> FindAll(int skip = 0, int take = 100) => _context.Entries
            .Skip(skip)
            .Take(take)
            .OrderBy(e => e.Id)
            .ToListAsync();

    }
}
