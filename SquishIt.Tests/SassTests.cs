using System;
using NUnit.Framework;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Sass;
using SquishIt.Tests.Helpers;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests
{
    [TestFixture(Category = "IgnoreCI", Description = "Assembly loading issues on build server.")]
    public abstract class SassTests
    {
        private readonly Func<CssBundleFactory> _cssBundleFactoryCreator;

        string scss = @"$blue: #3bbfce;
                    $margin: 16px;

                    .content-navigation {
                      border-color: $blue;
                      color:
                        darken($blue, 9%);
                    }

                    .border {
                      padding: $margin / 2;
                      margin: $margin / 2;
                      border-color: $blue;
                    }";
        string sass = @"$blue: #3bbfce
$margin: 16px

.content-navigation
    border-color: $blue
    color: darken($blue, 9%)

.border
    padding: $margin / 2
    margin: $margin / 2
    border-color: $blue";
        private const string RenderedCss = @".content-navigation{border-color:#3bbfce;color:#2ca2af}.border{padding:8px;margin:8px;border-color:#3bbfce}";
        private readonly string _debugRenderedCss = TestUtilities.NormalizeLineEndings(
            @"<style type=""text/css"">.content-navigation {
  border-color: #3bbfce;
  color: #2ca2af; }

.border {
  padding: 8px;
  margin: 8px;
  border-color: #3bbfce; }
</style>") + Environment.NewLine; //account for stringbuilder

        protected SassTests(Func<CssBundleFactory> cssBundleFactoryCreator)
        {
            _cssBundleFactoryCreator = cssBundleFactoryCreator;
        }

        [Test]
        public void CanBundleCssWithScss()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .WithContents(scss)
                    .Create();

                var tag = cssBundle
                    .Add("~/css/test.scss")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual(@"<link rel=""stylesheet"" type=""text/css"" href=""css/output.css?r=5C851B7837C923C399A44B1F5BF9F14A"" />", tag);
                Assert.AreEqual(RenderedCss, contents);
            }
        }

        [Test]
        public void CanBundleCssWithArbitraryScss()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .Create();

                var tag = cssBundle
                    .AddString(scss, ".scss")
                    .Render("~/css/output.css");

                var contents = _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual(@"<link rel=""stylesheet"" type=""text/css"" href=""css/output.css?r=5C851B7837C923C399A44B1F5BF9F14A"" />", tag);
                Assert.AreEqual(RenderedCss, contents);
            }
        }

        [Test]
        public void CanBundleCssInDebugWithArbitraryScss()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(true)
                    .Create();

                var tag = cssBundle
                    .AddString(scss, ".scss")
                    .Render("~/css/output.css");

                Assert.AreEqual(_debugRenderedCss, tag);
            }
        }

        [Test, Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleCssWithSass()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .WithContents(sass)
                    .Create();

                var tag = cssBundle
                    .Add("~/css/test.sass")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual(@"<link rel=""stylesheet"" type=""text/css"" href=""css/output.css?r=5C851B7837C923C399A44B1F5BF9F14A"" />", tag);
                Assert.AreEqual(RenderedCss, contents);
            }
        }

        [Test, Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleCssWithArbitrarySass()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .Create();

                var tag = cssBundle
                    .AddString(sass, ".sass")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual(@"<link rel=""stylesheet"" type=""text/css"" href=""css/output.css?r=5C851B7837C923C399A44B1F5BF9F14A"" />", tag);
                Assert.AreEqual(RenderedCss, contents);
            }
        }

        [Test, Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleCssInDebugWithArbitrarySass()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(true)
                    .Create();

                var tag = cssBundle
                    .AddString(sass, ".sass")
                    .Render("~/css/output.css");

                Assert.AreEqual(_debugRenderedCss, tag);
            }
        }

        [Test]
        public void CanUseNesting()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var original =
                    @"table.hl {
                      margin: 2em 0;
                      td.ln {
                        text-align: right;
                      }
                    }

                    li {
                      font: {
                        family: serif;
                        weight: bold;
                        size: 1.2em;
                      }
                    }";

                var expected =
                    @"table.hl{margin:2em 0}table.hl td.ln{text-align:right}li{font-family:serif;font-weight:bold;font-size:1.2em}";

                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .WithContents(original)
                    .Create();

                cssBundle
                    .Add("~/css/test.scss")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];
                Assert.AreEqual(expected, contents);
            }
        }

        [Test]
        public void CanUseMixins()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                var original =
                    @"@mixin table-base {
                      th {
                        text-align: center;
                        font-weight: bold;
                      }
                      td, th {padding: 2px}
                    }

                    @mixin left($dist) {
                      float: left;
                      margin-left: $dist;
                    }

                    #data {
                      @include left(10px);
                      @include table-base;
                    }";

                var expected =
                    @"#data{float:left;margin-left:10px}#data th{text-align:center;font-weight:bold}#data td,#data th{padding:2px}";

                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .WithContents(original)
                    .Create();

                cssBundle
                    .Add("~/css/test.scss")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];
                Assert.AreEqual(expected, contents);
            }
        }

        [Test]
        public void CanUseSelectorInheritance()
        {
            using (new StylePreprocessorScope<SassPreprocessor>())
            {
                const string original = @".error {
                      border: 1px #f00;
                      background: #fdd;
                    }
                    .error.intrusion {
                      font-size: 1.3em;
                      font-weight: bold;
                    }

                    .badError {
                      @extend .error;
                      border-width: 3px;
                    }";

                const string expected = @".error,.badError{border:1px #f00;background:#fdd}.error.intrusion,.intrusion.badError{font-size:1.3em;font-weight:bold}.badError{border-width:3px}";

                var cssBundle = _cssBundleFactoryCreator()
                    .WithHasher(new Hasher(new RetryableFileOpener()))
                    .WithDebuggingEnabled(false)
                    .WithContents(original)
                    .Create();

                cssBundle
                    .Add("~/css/test.scss")
                    .Render("~/css/output.css");

                var contents =
                    _cssBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"css\output.css")];

                Assert.AreEqual(expected, contents);
            }
        }
    }
}