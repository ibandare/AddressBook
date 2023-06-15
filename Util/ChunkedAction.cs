using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBook.Util
{
    internal class ChunkedAction
    {
        public static async Task Invoke<T>(Func<int, int, Task<List<T>>> provider, Func<IEnumerable<T>, Task> action, int take = 10000)
        {
            int skip = 0;
            int loaded;
            List<Task> tasks = new List<Task>();
            do
            {
                List<T> items = await provider.Invoke(skip, take);
                loaded = items.Count;
                skip += loaded;
                tasks.Add(action.Invoke(items));
            } while (loaded == take);
            await Task.WhenAll(tasks);
        }
    }
}
