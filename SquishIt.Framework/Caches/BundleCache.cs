using SquishIt.Framework.Caching;

namespace SquishIt.Framework.Caches
{
    public class BundleContentCache: ContentCache
    {
        protected override string KeyPrefix
        {
            get { return "squishit_"; }
        }

        public BundleContentCache(ICache cache) : base(cache)
        {
        }
    }
}