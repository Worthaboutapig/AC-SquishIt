using System.Collections;
using System.Runtime.Caching;

namespace SquishIt.Framework.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICache : IEnumerable
    {
        /// <summary>
        /// Gets or sets the cache item at the specified key.
        /// </summary>
        /// <param name="key">A String object that represents the key for the cache item.</param>
        /// <returns>The specified cache item.</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">A String identifier for the cache item to remove.</param>
        /// <returns>The item removed from the cache. If the value in the key parameter is not found, returns null.</returns>
        object Remove(string key);

        /// <summary>
        /// Adds the specified item to the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="cacheItemPolicy">The policy for the cache item.</param>
        /// <returns>An object that represents the item that was added if the item was previously stored in the cache; otherwise, null.</returns>
        object Add(string key, object value, CacheItemPolicy cacheItemPolicy);
    }
}