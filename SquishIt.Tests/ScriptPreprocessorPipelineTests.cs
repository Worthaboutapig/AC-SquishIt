using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Framework.Base;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Helpers;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests
{
    public abstract class ScriptPreprocessorPipelineTests
    {
        JavaScriptBundleFactory javaScriptBundleFactory;
        IHasher hasher;
        private readonly string _baseOutputHref;
        private readonly IPathTranslator _pathTranslator;
        private readonly IFolderResolver _fileSystemResolver;
        private readonly IFileResolver _httpResolver;
        private readonly IFileResolver _rootEmbeddedResourceResolver;
        private readonly IFileResolver _standardEmbeddedResourceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ScriptPreprocessorPipelineTests(string baseOutputHref, IPathTranslator pathTranslator, IFolderResolver fileSystemResolver, IFileResolver httpResolver, IFileResolver rootEmbeddedResourceResolver, IFileResolver standardEmbeddedResourceResolver)
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
        public void Setup()
        {
            javaScriptBundleFactory = new JavaScriptBundleFactory(_baseOutputHref, _pathTranslator, _fileSystemResolver, _httpResolver, _rootEmbeddedResourceResolver, _standardEmbeddedResourceResolver);
            hasher = new StubHasher("hash");
        }

        [Test]
        public void Js_Style_Then_Global()
        {
            var scriptPreprocessor = new StubScriptPreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                JavaScriptBundle javaScriptBundle = javaScriptBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = javaScriptBundle
                    .Add("~/js/test.global.script")
                    .Render("~/js/output.js");

                string contents =
                    javaScriptBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

                Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
                Assert.AreEqual("globey;\n", contents);

                Assert.AreEqual("start", scriptPreprocessor.CalledWith);
                Assert.AreEqual("scripty", globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void Js_Global_Then_Style()
        {
            var scriptPreprocessor = new StubScriptPreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                JavaScriptBundle javaScriptBundle = javaScriptBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = javaScriptBundle
                    .Add("~/js/test.script.global")
                    .Render("~/js/output.js");

                string contents =
                    javaScriptBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

                Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
                Assert.AreEqual("scripty;\n", contents);

                Assert.AreEqual("globey", scriptPreprocessor.CalledWith);
                Assert.AreEqual("start", globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void Js_Stops_At_First_Extension_With_No_Defined_Preprocessor()
        {
            var scriptPreprocessor = new StubScriptPreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                JavaScriptBundle javaScriptBundle = javaScriptBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = javaScriptBundle
                    .Add("~/js/test.script.fake.global.bogus")
                    .Render("~/js/output.js");

                string contents =
                    javaScriptBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

                Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
                Assert.AreEqual("start;\n", contents);

                Assert.Null(scriptPreprocessor.CalledWith);
                Assert.Null(globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void WithPreprocessor_Uses_Instance_Preprocessors()
        {
            var scriptPreprocessor = new StubScriptPreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            JavaScriptBundle javaScriptBundle = javaScriptBundleFactory
                .WithHasher(hasher)
                .WithDebuggingEnabled(false)
                .WithContents("start")
                .Create();

            string tag = javaScriptBundle
                .WithPreprocessor(scriptPreprocessor)
                .WithPreprocessor(globalPreprocessor)
                .Add("~/js/test.script.global")
                .Render("~/js/output.js");

            string contents =
                javaScriptBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

            Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
            Assert.AreEqual("scripty;\n", contents);

            Assert.AreEqual("globey", scriptPreprocessor.CalledWith);
            Assert.AreEqual("start", globalPreprocessor.CalledWith);

            Assert.IsEmpty(Bundle.Preprocessors.Where(x => !(x is NullPreprocessor)));
        }
    }
}
