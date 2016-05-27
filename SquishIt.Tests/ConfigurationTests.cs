using System;
using Moq;
using NUnit.Framework;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Helpers;

namespace SquishIt.Tests
{
    public abstract class ConfigurationTests
    {
        private readonly Func<JavaScriptBundleFactory> _javaScriptBundleFactory;
        private readonly Func<CssBundleFactory> _cssBundleFactory;

        protected ConfigurationTests(Func<JavaScriptBundleFactory> javaScriptBundleFactory, Func<CssBundleFactory> cssBundleFactory)
        {
            _javaScriptBundleFactory = javaScriptBundleFactory;
            _cssBundleFactory = cssBundleFactory;
        }

        [Test]
        public void WithHasher()
        {
            var hasher = new Mock<IHasher>();
            hasher.Setup(h => h.GetHash(It.IsAny<string>())).Returns("pizza");

            var trustLevel = new Mock<ITrustLevel>();
            trustLevel.SetupGet(tl => tl.IsFullTrust).Returns(true); //globally configured hasher is used another 2 times in high / full trust when obtaining mutex

            var javaScriptBundle = _javaScriptBundleFactory()
                .WithDebuggingEnabled(false)
                .WithTrustLevel(trustLevel.Object)
                .WithHasher(hasher.Object)
                .Create();

            var jsTag = javaScriptBundle
                            .AddString("test")
                            .Render("configured-hash.js");

            Assert.True(jsTag.Contains("?r=pizza"));

            var cssBundle = _cssBundleFactory()
                .WithDebuggingEnabled(false)
                .WithTrustLevel(trustLevel.Object)
                .WithHasher(hasher.Object)
                .Create();

            var cssTag = cssBundle
                                .AddString("test")
                                .Render("configured-hash.css");

            Assert.True(cssTag.Contains("?r=pizza"));

            hasher.Verify(f => f.GetHash(It.Is<string>(s => s.EndsWith(".js"))), Times.Once());//js bundle rendering mutex
            hasher.Verify(h => h.GetHash(It.Is<string>(s => s.StartsWith("test;"))), Times.Once());//js content

            hasher.Verify(f => f.GetHash(It.Is<string>(s => s.EndsWith(".css"))), Times.Once());//css bundle rendering mutex
            hasher.Verify(h => h.GetHash("test"), Times.Once());//css content
        }
    }
}
