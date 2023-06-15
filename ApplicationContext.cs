using AddressBook.Model;
using Microsoft.EntityFrameworkCore;

namespace AddressBook
{
    public class ApplicationContext : DbContext
    {
        internal DbSet<Entry> Entries { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
