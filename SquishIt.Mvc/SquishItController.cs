using System;
using System.Web.Mvc;

namespace SquishIt.Mvc
{
    using Framework.CSS;
    using Framework.JavaScript;

    public class SquishItController : Controller
    {
        public SquishItController(Func<CSSBundle> cssBundleCreator, Func<JavaScriptBundle> javaScriptBundleCreator, string cssMimeType = "text/css", string javascriptMimeType = "application/javascript")
        {
            _cssBundleCreator = cssBundleCreator;
            _javaScriptBundleCreator = javaScriptBundleCreator;

            _cssMimeType = cssMimeType;
            _javascriptMimeType = javascriptMimeType;
        }

        private readonly string _javascriptMimeType;
        private readonly string _cssMimeType;
        private readonly Func<CSSBundle> _cssBundleCreator;
        private readonly Func<JavaScriptBundle> _javaScriptBundleCreator;

        public ActionResult Js(string id)
        {
            var renderCached = _javaScriptBundleCreator().RenderCached(id);
            var content = Content(renderCached, _javascriptMimeType);

            return content;
        }

        public ActionResult Css(string id)
        {
            var renderCached = _cssBundleCreator().RenderCached(id);
            var content = Content(renderCached, _cssMimeType);

            return content;
        }
    }
}