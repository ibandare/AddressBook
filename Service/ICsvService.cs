using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBook.Service
{
    public interface ICsvService
    {
        Task ReadCsv<T>(string filePath, Func<IEnumerable<T>, Task> action);
    }
}
