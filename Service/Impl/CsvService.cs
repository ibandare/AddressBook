using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Service.Impl
{
    public class CsvService : ICsvService
    {
        private readonly CsvConfiguration configuration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ",",
        };

        public async Task ReadCsv<T>(string filePath, Func<IEnumerable<T>, Task> action)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, configuration);
            IAsyncEnumerable<T> data = csv.GetRecordsAsync<T>();

            int take = 10000;
            int loaded;
            
            do
            {
                List<T> items = await data.Take(take).ToListAsync();
                loaded = items.Count;
                await action.Invoke(items);
            } while (loaded == take);
        }
    }
}
