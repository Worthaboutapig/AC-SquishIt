using SquishIt.Mvc;

namespace SquishItAspNetMvcTest.Controllers
{
    using SquishIt.Framework.Utilities;

    public class AssetCacheController : SquishItController
    {
        public AssetCacheController(IDebugStatusReader debugStatusReader) : base(debugStatusReader)
        {
        }
    }
}