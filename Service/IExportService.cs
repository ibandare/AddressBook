using AddressBook.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBook.Service
{
    public interface IExportService
    {
        Task Export(Func<int, int, Task<List<Entry>>> provider);

        bool Supports(string token);
    }
}
