using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Service.Impl
{
    internal class ExportServiceFactory : IExportServiceFactory
    {
        private readonly IEnumerable<IExportService> services;

        public ExportServiceFactory(IEnumerable<IExportService> services)
        {
            this.services = services;
        }

        public IExportService GetInstance(string token) => services.FirstOrDefault(x => x.Supports(token));
    }
}
