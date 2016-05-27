using System;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Tests.Helpers;

namespace SquishIt.Tests
{
    public abstract class CoffeeScriptTests
    {
        private readonly Func<JavaScriptBundleFactory> _javaScriptBundleFactory;
        private readonly IPathTranslator _pathTranslator;

        protected CoffeeScriptTests(Func<JavaScriptBundleFactory> javaScriptBundleFactory, IPathTranslator pathTranslator)
        {
            _javaScriptBundleFactory = javaScriptBundleFactory;
            _pathTranslator = pathTranslator;
        }

        [TestCase(typeof(MsIeCoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        [TestCase(typeof(CoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleJavascriptWithArbitraryCoffeeScript(Type preprocessorType)
        {
            var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
            Assert.NotNull(preprocessor);

            const string coffee = "alert 'test' ";
            const string path = "~/brewed.js";

            var javaScriptBundleFactory = _javaScriptBundleFactory();
            var tag = javaScriptBundleFactory
                .WithDebuggingEnabled(false)
                .Create()
                .WithPreprocessor(preprocessor)
                .AddString(coffee, ".coffee")
                .Render(path);

            //var prepareRelativePath = TestUtilities.PrepareRelativePath(@"brewed.js");

            var resolvedPath = _pathTranslator.ResolveAppRelativePathToFileSystem(path);
            var compiled = javaScriptBundleFactory.FileWriterFactory.Files[resolvedPath];

            Assert.AreEqual("(function(){alert(\"test\")}).call(this);\n", compiled);
            Assert.AreEqual(@"<script type=""text/javascript"" src=""/brewed.js?r=hash""></script>", tag);
        }

        [TestCase(typeof(MsIeCoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        [TestCase(typeof(CoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleJavascriptInDebugWithArbitraryCoffeeScript(Type preprocessorType)
        {
            var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
            Assert.NotNull(preprocessor);

            const string coffee = "alert 'test' ";

            var tag = _javaScriptBundleFactory()
                .WithDebuggingEnabled(true)
                .Create()
                .WithPreprocessor(preprocessor)
                .AddString(coffee, ".coffee")
                .Render("~/brewed.js");

            Assert.AreEqual("<script type=\"text/javascript\">(function() {\n  alert('test');\n\n}).call(this);\n</script>\n", TestUtilities.NormalizeLineEndings(tag));
        }
    }
}