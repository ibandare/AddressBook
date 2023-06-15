using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Service
{
    public interface IExportServiceFactory
    { 
        IExportService GetInstance(string token);
    }
}
