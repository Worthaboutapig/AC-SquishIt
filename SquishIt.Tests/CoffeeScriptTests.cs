using System;
using System.Diagnostics.Contracts;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Framework.Resolvers;
using SquishIt.Tests.Helpers;

namespace SquishIt.Tests
{
    public abstract class CoffeeScriptTests
    {
        JavaScriptBundleFactory javaScriptBundleFactory;
        private readonly string _baseOutputHref;
        private readonly IPathTranslator _pathTranslator;
        private readonly IFolderResolver _fileSystemResolver;
        private readonly IFileResolver _httpResolver;
        private readonly IFileResolver _rootEmbeddedResourceResolver;
        private readonly IFileResolver _standardEmbeddedResourceResolver;

        protected CoffeeScriptTests(string baseOutputHref, IPathTranslator pathTranslator, IFolderResolver fileSystemResolver, IFileResolver httpResolver, IFileResolver rootEmbeddedResourceResolver, IFileResolver standardEmbeddedResourceResolver)
        {
            Contract.Requires(baseOutputHref != null);
            Contract.Requires(pathTranslator != null);
            Contract.Requires(fileSystemResolver != null);
            Contract.Requires(httpResolver != null);
            Contract.Requires(rootEmbeddedResourceResolver != null);
            Contract.Requires(standardEmbeddedResourceResolver != null);

            Contract.Ensures(_baseOutputHref != null);
            Contract.Ensures(_pathTranslator != null);
            Contract.Ensures(_fileSystemResolver != null);
            Contract.Ensures(_httpResolver != null);
            Contract.Ensures(_rootEmbeddedResourceResolver != null);
            Contract.Ensures(_standardEmbeddedResourceResolver != null);

            _baseOutputHref = baseOutputHref;
            _pathTranslator = pathTranslator;
            _fileSystemResolver = fileSystemResolver;
            _httpResolver = httpResolver;
            _rootEmbeddedResourceResolver = rootEmbeddedResourceResolver;
            _standardEmbeddedResourceResolver = standardEmbeddedResourceResolver;
        }

        [SetUp]
        public virtual void Setup()
        {
            javaScriptBundleFactory = new JavaScriptBundleFactory(_baseOutputHref, _pathTranslator, _fileSystemResolver, _httpResolver, _rootEmbeddedResourceResolver, _standardEmbeddedResourceResolver);
        }

        [TestCase(typeof(MsIeCoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        [TestCase(typeof(CoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleJavascriptWithArbitraryCoffeeScript(Type preprocessorType)
        {
            var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
            Assert.NotNull(preprocessor);

            var coffee = "alert 'test' ";

            var tag = javaScriptBundleFactory
                .WithDebuggingEnabled(false)
                .Create()
                .WithPreprocessor(preprocessor)
                .AddString(coffee, ".coffee")
                .Render("~/brewed.js");

            var compiled = javaScriptBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"brewed.js")];

            Assert.AreEqual("(function(){alert(\"test\")}).call(this);\n", compiled);
            Assert.AreEqual(@"<script type=""text/javascript"" src=""brewed.js?r=hash""></script>", tag);
        }

        [TestCase(typeof(MsIeCoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        [TestCase(typeof(CoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleJavascriptInDebugWithArbitraryCoffeeScript(Type preprocessorType)
        {
            var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
            Assert.NotNull(preprocessor);

            var coffee = "alert 'test' ";

            var tag = javaScriptBundleFactory
                .WithDebuggingEnabled(true)
                .Create()
                .WithPreprocessor(preprocessor)
                .AddString(coffee, ".coffee")
                .Render("~/brewed.js");

            Assert.AreEqual("<script type=\"text/javascript\">(function() {\n  alert('test');\n\n}).call(this);\n</script>\n", TestUtilities.NormalizeLineEndings(tag));
        }
    }
}
