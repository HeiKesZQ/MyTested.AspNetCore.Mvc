﻿namespace MyTested.Mvc.Internal.Caching
{
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Caching.Memory;
    using Contracts;

#if NET451
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting;
#elif DOTNET5_6
    using System.Threading;
#endif

    public class MockedMemoryCache : IMockedMemoryCache
    {
#if NET451
        private const string DataKey = "__MemoryCache_Current__";
#elif DOTNET5_6
        private static readonly AsyncLocal<IDictionary<object, IMockedCacheEntry>> МemoryCacheCurrent = new AsyncLocal<IDictionary<object, IMockedCacheEntry>>();
#endif
        private readonly IDictionary<object, IMockedCacheEntry> cache;

        public MockedMemoryCache()
        {
            this.cache = this.GetCurrentCache();
        }

        public IEntryLink CreateLinkingScope()
        {
            return new MockedEntryLink();
        }

        public void Dispose()
        {
            this.cache.Clear();
        }

        public void Remove(object key)
        {
            if (this.cache.ContainsKey(key))
            {
                this.cache.Remove(key);
            }
        }

        public object Set(object key, object value, MemoryCacheEntryOptions options)
        {
            this.cache[key] = new MockedCacheEntry(key, value, options);
            return value;
        }

        public bool TryGetValue(object key, out object value)
        {
            IMockedCacheEntry cacheEntry;
            if (this.TryGetCacheEntry(key, out cacheEntry))
            {
                value = cacheEntry.Value;
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetCacheEntry(object key, out IMockedCacheEntry value)
        {
            if (this.cache.ContainsKey(key))
            {
                value = this.cache[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public IDictionary<object, object> GetCacheAsDictionary()
        {
            return this.cache.ToDictionary(c => c.Key, c => c.Value.Value);
        }

        private IDictionary<object, IMockedCacheEntry> GetCurrentCache()
        {
#if NET451
            var handle = CallContext.GetData(DataKey) as ObjectHandle;
            var result = handle?.Unwrap() as IDictionary<object, IMockedCacheEntry>;
            if (result == null)
            {
                result = new Dictionary<object, IMockedCacheEntry>();
                CallContext.SetData(DataKey, new ObjectHandle(result));
            }

            return result;
#elif DOTNET5_6
            var result = МemoryCacheCurrent.Value;
            if (result == null)
            {
                result = new Dictionary<object, IMockedCacheEntry>();
                МemoryCacheCurrent.Value = result;
            }

            return result;
#endif
        }
    }
}