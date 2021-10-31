using Microsoft.Extensions.Caching.Memory;
using MoviesVB.Core;
using System;
using System.Collections.Generic;

namespace MoviesVB.DomainServices
{
    public class MemCacheService : IMemCacheService
    {
        private const int ExpirationScanFrequencyInMinutes = 60;
        private readonly IMemoryCache _memoryCache;

        public MemCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public MemCacheService()
        {
            var cacheOptions = new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(ExpirationScanFrequencyInMinutes)
            };

            _memoryCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// Add object to the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="sliding">Sliding if true and Absolute if false</param>
        public void Add<T>(string key, T obj, TimeSpan cacheDuration, bool sliding = true)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();

            if (sliding)
            {
                cacheEntryOptions.SetSlidingExpiration(cacheDuration);
            }
            else
            {
                cacheEntryOptions.SetAbsoluteExpiration(cacheDuration);
            }

            _memoryCache.Set(key, obj, cacheEntryOptions);
        }

        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue<T>(key, out var value))
            {
                return value;
            }

            return default(T);
        }

        public IList<T> GetAll<T>(IEnumerable<string> keys)
        {
            var values = new List<T>();

            foreach (var key in keys)
            {
                if (_memoryCache.TryGetValue<T>(key, out var value))
                {
                    values.Add(value);
                }
            }

            return values;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
