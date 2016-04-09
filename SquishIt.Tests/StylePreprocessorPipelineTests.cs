using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Framework.Base;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Tests.Helpers;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests
{
    public abstract class StylePreprocessorPipelineTests
    {
        CssBundleFactory cssBundleFactory;
        IHasher hasher;
        private readonly IHttpUtility _httpUtility;
        private readonly string _baseOutputHref;
        private readonly IPathTranslator _pathTranslator;
        private readonly FileSystemResolver _fileSystemResolver;
        private readonly HttpResolver _httpResolver;
        private readonly RootEmbeddedResourceResolver _rootEmbeddedResourceResolver;
        private readonly StandardEmbeddedResourceResolver _standardEmbeddedResourceResolver;


        protected StylePreprocessorPipelineTests(IHttpUtility httpUtility, string baseOutputHref, IPathTranslator pathTranslator, FileSystemResolver fileSystemResolver, HttpResolver httpResolver, RootEmbeddedResourceResolver rootEmbeddedResourceResolver, StandardEmbeddedResourceResolver standardEmbeddedResourceResolver)
        {
            Contract.Requires(httpUtility != null);
            Contract.Requires(baseOutputHref != null);
            Contract.Requires(pathTranslator != null);
            Contract.Requires(fileSystemResolver != null);
            Contract.Requires(httpResolver != null);
            Contract.Requires(rootEmbeddedResourceResolver != null);
            Contract.Requires(standardEmbeddedResourceResolver != null);


            Contract.Ensures(_httpUtility != null); Contract.Ensures(_baseOutputHref != null);
            Contract.Ensures(_pathTranslator != null);
            Contract.Ensures(_fileSystemResolver != null);
            Contract.Ensures(_httpResolver != null);
            Contract.Ensures(_rootEmbeddedResourceResolver != null);
            Contract.Ensures(_standardEmbeddedResourceResolver != null);

            _httpUtility = httpUtility;
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
            cssBundleFactory = new CssBundleFactory(_httpUtility, _baseOutputHref, _pathTranslator, _fileSystemResolver, _httpResolver, _rootEmbeddedResourceResolver, _standardEmbeddedResourceResolver);
            hasher = new StubHasher("hash");
        }

        [Test]
        public void Css_Style_Then_Global()
        {
            var stylePreprocessor = new StubStylePreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new StylePreprocessorScope<StubStylePreprocessor>(stylePreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                CSSBundle cssBundle = cssBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = cssBundle
                    .Add("~/css/test.global.style")
                    .Render("~/css/output.css");

                string contents =
                    cssBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"css/output.css?r=hash\" />", tag);
                Assert.AreEqual("globey", contents);

                Assert.AreEqual("start", stylePreprocessor.CalledWith);
                Assert.AreEqual("styley", globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void Css_Global_Then_Style()
        {
            var stylePreprocessor = new StubStylePreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new StylePreprocessorScope<StubStylePreprocessor>(stylePreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                CSSBundle cssBundle = cssBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = cssBundle
                    .Add("~/css/test.style.global")
                    .Render("~/css/output.css");

                string contents =
                    cssBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"css/output.css?r=hash\" />", tag);
                Assert.AreEqual("styley", contents);

                Assert.AreEqual("globey", stylePreprocessor.CalledWith);
                Assert.AreEqual("start", globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void Css_Stops_At_First_Extension_With_No_Defined_Preprocessor()
        {
            var stylePreprocessor = new StubStylePreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            using(new StylePreprocessorScope<StubStylePreprocessor>(stylePreprocessor))
            using(new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
            {
                CSSBundle cssBundle = cssBundleFactory
                    .WithHasher(hasher)
                    .WithDebuggingEnabled(false)
                    .WithContents("start")
                    .Create();

                string tag = cssBundle
                    .Add("~/css/test.style.fake.global.bogus")
                    .Render("~/css/output.css");

                string contents =
                    cssBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"css/output.css?r=hash\" />", tag);
                Assert.AreEqual("start", contents);

                Assert.Null(stylePreprocessor.CalledWith);
                Assert.Null(globalPreprocessor.CalledWith);
            }
        }

        [Test]
        public void WithPreprocessor_Uses_Instance_Preprocessors()
        {
            var stylePreprocessor = new StubStylePreprocessor();
            var globalPreprocessor = new StubGlobalPreprocessor();

            CSSBundle cssBundle = cssBundleFactory
                .WithHasher(hasher)
                .WithDebuggingEnabled(false)
                .WithContents("start")
                .Create();

            string tag = cssBundle
                .Add("~/css/test.style.global")
                .WithPreprocessor(stylePreprocessor)
                .WithPreprocessor(globalPreprocessor)
                .Render("~/css/output.css");

            string contents =
                cssBundleFactory.FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"css/output.css?r=hash\" />", tag);
            Assert.AreEqual("styley", contents);

            Assert.AreEqual("globey", stylePreprocessor.CalledWith);
            Assert.AreEqual("start", globalPreprocessor.CalledWith);

            Assert.IsEmpty(Bundle.Preprocessors.Where(x => !(x is NullPreprocessor)));
        }
    }
}
