using System.Diagnostics.Contracts;
using System.Web.Mvc;
using SquishIt.Framework;
using SquishIt.Framework.Utilities;

namespace SquishIt.Mvc
{
    public class SquishItController : Controller
    {
        public SquishItController(IDebugStatusReader debugStatusReader)
        {
            Contract.Requires(debugStatusReader != null);

            Contract.Requires(_debugStatusReader != null);

            _debugStatusReader = debugStatusReader;
        }

        private readonly IDebugStatusReader _debugStatusReader;

        public ActionResult Js(string id)
        {
            var renderCached = Bundle.JavaScript(_debugStatusReader).RenderCached(id);
            var content = Content(renderCached, Configuration.Instance.JavascriptMimeType);

            return content;
        }

        public ActionResult Css(string id)
        {
            var renderCached = Bundle.Css(_debugStatusReader).RenderCached(id);
            var content = Content(renderCached, Configuration.Instance.CssMimeType);

            return content;
        }
    }
}