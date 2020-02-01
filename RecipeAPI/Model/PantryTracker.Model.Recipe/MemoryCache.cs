﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryTracker.Model
{
    public class MemoryCache : ICacheManager
    {
        private const int DefaultCacheDurationInMinutes = 60;

        private readonly Dictionary<string, Tuple<object, DateTime>> _cache;

        public MemoryCache()
        {
            _cache = new Dictionary<string, Tuple<object, DateTime>>();
        }

        public void Add<T>(string key, T toCache, TimeSpan validity = default)
        {
            if(validity == default)
            {
                _ = TimeSpan.FromMinutes(DefaultCacheDurationInMinutes);
            }

            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cache key is required.");
            }

            _cache[key] = new Tuple<object, DateTime>(toCache, DateTime.Now.Add(validity));
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> refreshDelegate, TimeSpan validity = default)
        {
            if(_cache.Keys.Contains(key))
            {
                var val = _cache[key];
                if(val.Item2 >= DateTime.Now)
                {
                    return (T)val.Item1;
                }
            }

            var updatedVal = await refreshDelegate();
            Add(key, updatedVal, validity);
            return updatedVal;
        }

        public T Get<T>(string key, Func<T> refreshDelegate, TimeSpan validity = default)
        {
            if (_cache.Keys.Contains(key))
            {
                var val = _cache[key];
                if (val.Item2 >= DateTime.Now)
                {
                    return (T)val.Item1;
                }
            }

            var updatedVal = refreshDelegate();
            Add(key, updatedVal, validity);
            return updatedVal;
        }
    }
}