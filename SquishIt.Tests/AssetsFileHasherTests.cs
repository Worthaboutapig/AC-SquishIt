using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Resolvers;
using SquishIt.Tests.Stubs;
using SquishIt.Tests.Helpers;
using SquishIt.Framework.Web;

namespace SquishIt.Tests
{
    public abstract class AssetsFileHasherTests
    {
        private const string HashQueryStringKeyName = "v";
        private const string HashValue = "hashValue";

        private readonly IHttpUtility _httpUtility;
        private readonly IPathTranslator _pathTranslator;

        protected AssetsFileHasherTests(IHttpUtility httpUtility, IPathTranslator pathTranslator)
        {
            if (httpUtility == null)
            {
                throw new ArgumentNullException("httpUtility");
            }

            if (pathTranslator == null)
            {
                throw new ArgumentNullException("pathTranslator");
            }

            _pathTranslator = pathTranslator;
            _httpUtility = httpUtility;
        }

        [Test]
        public void DoesNotAppendHashIfFileIsRemote()
        {
            var fileResolver = new FileSystemResolver();
            var hasher = new StubHasher("hash");

            const string cssFilePath = @"C:\somepath\output.css";
            const string url = "http://www.test.com/image.jpg";

            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(url));
        }

        [Test]
        public void DoesNotAppendHashIfFileDoesNotExists()
        {
            var fileResolver = new FileSystemResolver();
            var hasher = new StubHasher("hash");

            var cssFilePath = TestUtilities.PreparePath(@"C:\somepath\output.css");
            const string url = "/doesnotexist.jpg";

            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(url));
        }

        [Test]
        public void CanAppendFileHashToRelativeUri()
        {
            var hasher = new StubHasher(HashValue);
            var fileResolver = new FileSystemResolver();

            var uri = Assembly.GetExecutingAssembly().CodeBase;
            var cssFilePath = Path.GetDirectoryName(uri) + TestUtilities.PreparePath(@"\subdirectory\output.css");
            var url = "../" + Path.GetFileName(uri);
            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var expectedUrl = url + "?" + HashQueryStringKeyName + "=" + HashValue;

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
        }

        [Test]
        public void CanAppendFileHashToRelativeUriWithAnExistingQueryString()
        {
            var hasher = new StubHasher(HashValue);
            var fileResolver = new FileSystemResolver();

            var uri = Assembly.GetExecutingAssembly().CodeBase;
            var cssFilePath = Path.GetDirectoryName(uri) + TestUtilities.PreparePath(@"\subdirectory\output.css");
            var url = "../" + Path.GetFileName(uri) + "?test=value";
            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var expectedUrl = url + "&" + HashQueryStringKeyName + "=" + HashValue;

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
        }

        [Test]
        public void CanAppendFileHashToAbsoluteUri()
        {
            var hasher = new StubHasher(HashValue);
            var uri = Assembly.GetExecutingAssembly().CodeBase;
            var cssFilePath = Path.Combine(Path.GetDirectoryName(uri), @"output.css");
            var url = "/" + Path.GetFileName(uri);
            var pathToResolveTo = Assembly.GetExecutingAssembly().Location;
            var fileResolver = StubFileResolver.ForFile(pathToResolveTo);

            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var expectedUrl = url + "?" + HashQueryStringKeyName + "=" + HashValue;

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
        }

        [Test]
        public void CanAppendFileHashToAbsoluteUriWithAnExistingQueryString()
        {
            var hasher = new StubHasher(HashValue);
            var uri = Assembly.GetExecutingAssembly().CodeBase;
            var cssFilePath = Path.GetDirectoryName(uri) + @"\output.css";
            var url = "/" + Path.GetFileName(uri) + "?test=value";
            var pathToResolveTo = Assembly.GetExecutingAssembly().Location;
            var fileResolver = StubFileResolver.ForFile(pathToResolveTo);

            var assetsFileHasher = new CSSAssetsFileHasher(HashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

            var expectedUrl = url + "&" + HashQueryStringKeyName + "=" + HashValue;

            var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

            Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
        }
    }
}