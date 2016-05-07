using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Caching;

namespace SquishIt.Framework
{
    public abstract class ContentCache : IContentCache
    {
        private readonly ICache _cache;
        private readonly List<string> CacheKeys = new List<string>();

        /// <summary>
        /// Initialize the content cache with an existing <paramref name="cache"/> and <paramref name="cacheDependencyFactory"/>.
        /// </summary>
        /// <param name="cache">An existing cache.</param>
        protected ContentCache(ICache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Used to prefix keys in the cache.
        /// </summary>
        protected abstract string KeyPrefix { get; }

        public string GetContent(string name)
        {
            return (string) _cache[KeyPrefix + name];
        }

        public void ClearTestingCache()
        {
            foreach (var key in CacheKeys)
            {
                _cache.Remove(key);
            }
        }

        public bool ContainsKey(string key)
        {
            return _cache[BuildCacheKey(key)] != null;
        }

        public bool TryGetValue(string key, out string content)
        {
            content = (string) _cache[BuildCacheKey(key)];
            return content != null;
        }

        public void Add(string key, string content, List<string> files, bool debuggingEnabled)
        {
            var cacheKey = BuildCacheKey(key);
            CacheKeys.Add(cacheKey);

            var cacheItemPolicy = new CacheItemPolicy
                                  {
                                      Priority = CacheItemPriority.NotRemovable,
                                      SlidingExpiration = new TimeSpan(365, 0, 0, 0),
                                      AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
                                  };
            if (!debuggingEnabled && files != null && files.Any())
            {
                var physicalFiles = new List<string>();
                physicalFiles.AddRange(files.Where(File.Exists));
                cacheItemPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(physicalFiles));
            }

            _cache.Add(cacheKey, content, cacheItemPolicy);
        }

        public void Remove(string key)
        {
            _cache.Remove(BuildCacheKey(key));
        }

        private string BuildCacheKey(string key)
        {
            return KeyPrefix + key;
        }
    }
}