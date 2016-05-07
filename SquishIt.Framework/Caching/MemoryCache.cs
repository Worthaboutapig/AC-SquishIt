using System.Runtime.Caching;

namespace SquishIt.Framework.Caching
{
    /// <summary>
    /// Creates a basic cache implementation.
    /// </summary>
    public class MemoryCache : System.Runtime.Caching.MemoryCache,
        ICache
    {
        private readonly string _regionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MemoryCache(string regionName = null) : base(regionName ?? RegionName)
        {
            _regionName = regionName ?? RegionName;
        }

        /// <summary>
        /// The region name.
        /// </summary>
        public const string RegionName = "DefaultSquishItCache";

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">A String identifier for the cache item to remove.</param>
        /// <returns>The item removed from the cache. If the value in the key parameter is not found, returns null.</returns>
        public object Remove(string key)
        {
            return base.Remove(key, _regionName);
        }

        /// <summary>
        /// Adds the specified item to the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="cacheItemPolicy">The policy for the cache item.</param>
        /// <returns>An object that represents the item that was added if the item was previously stored in the cache; otherwise, null.</returns>
        public object Add(string key, object value, CacheItemPolicy cacheItemPolicy)
        {
            return base.Add(key, value, cacheItemPolicy, _regionName);
        }
    }
}