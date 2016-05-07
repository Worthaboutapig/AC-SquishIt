using System.Diagnostics.Contracts;
using System.Web.Mvc;
using SquishIt.Framework.Utilities;

namespace SquishIt.Mvc
{
    using Framework.CSS;
    using Framework.JavaScript;

    public class SquishItController : Controller
    {
        public SquishItController(IDebugStatusReader debugStatusReader, string cssMimeType = "text/css", string javascriptMimeType = "application/javascript")
        {
            Contract.Requires(debugStatusReader != null);

            Contract.Requires(_debugStatusReader != null);

            _debugStatusReader = debugStatusReader;
            _cssMimeType = cssMimeType;
            _javascriptMimeType = javascriptMimeType;
        }

        private readonly IDebugStatusReader _debugStatusReader;
        private readonly string _javascriptMimeType;
        private readonly string _cssMimeType;

        public ActionResult Js(string id)
        {
            var renderCached = new JavaScriptBundle(_debugStatusReader).RenderCached(id);
            var content = Content(renderCached, _javascriptMimeType);

            return content;
        }

        public ActionResult Css(string id)
        {
            var renderCached = new CSSBundle(_debugStatusReader).RenderCached(id);
            var content = Content(renderCached, _cssMimeType);

            return content;
        }
    }
}