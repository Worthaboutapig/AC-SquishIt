using SquishIt.Framework.Caching;

namespace SquishIt.Framework.Caches
{
    public class BundleCache: ContentCache
    {
        protected override string KeyPrefix
        {
            get { return "squishit_"; }
        }

        public BundleCache(ICache cache) : base(cache)
        {
        }
    }
}