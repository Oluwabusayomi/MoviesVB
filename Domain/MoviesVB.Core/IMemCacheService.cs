using System;
using System.Collections.Generic;

namespace MoviesVB.Core
{
    public interface IMemCacheService
    {
        void Add<T>(string key, T obj, TimeSpan cacheDuration, bool sliding = true);

        void Remove(string key);

        T Get<T>(string key);

        IList<T> GetAll<T>(IEnumerable<string> keys);
    }
}
