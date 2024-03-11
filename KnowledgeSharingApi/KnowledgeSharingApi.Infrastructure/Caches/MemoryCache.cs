using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Caches
{
    public class MemoryCache(IMemoryCache cache) : ICache
    {
        protected IMemoryCache Cache = cache;

        public bool Contains(string key)
        {
            return Cache.Get<object>(key) != null;
        }

        public object? Get(string key)
        {
            return Cache.Get<object>(key);
        }

        public T? Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        public ICache GetInstance()
        {
            return this;
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Set(string key, object value)
        {
            Cache.Set(key, value);
        }

        public void Set<T>(string key, T value)
        {
            Cache.Set<T>(key, value);
        }
    }
}
