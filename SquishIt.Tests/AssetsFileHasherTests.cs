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
	using System;

	public abstract class AssetsFileHasherTests
	{
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
			var hashQueryStringKeyName = "v";
			var fileResolver = new FileSystemResolver();
			var hasher = new StubHasher("hash");

			var cssFilePath = @"C:\somepath\output.css";
			var url = "http://www.test.com/image.jpg";

			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(url));
		}

		[Test]
		public void DoesNotAppendHashIfFileDoesNotExists()
		{
			var hashQueryStringKeyName = "v";
			var fileResolver = new FileSystemResolver();
			var hasher = new StubHasher("hash");

			var cssFilePath = TestUtilities.PreparePath(@"C:\somepath\output.css");
			var url = "/doesnotexist.jpg";

			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(url));
		}

		[Test]
		public void CanAppendFileHashToRelativeUri()
		{
			var hashQueryStringKeyName = "v";
			var hashValue = "hashValue";
			var hasher = new StubHasher(hashValue);
			var fileResolver = new FileSystemResolver();

			var uri = Assembly.GetExecutingAssembly().CodeBase;
			var cssFilePath = Path.GetDirectoryName(uri) + TestUtilities.PreparePath(@"\subdirectory\output.css");
			var url = "../" + Path.GetFileName(uri);
			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var expectedUrl = url + "?" + hashQueryStringKeyName + "=" + hashValue;

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
		}

		[Test]
		public void CanAppendFileHashToRelativeUriWithAnExistingQueryString()
		{
			var hashQueryStringKeyName = "v";
			var hashValue = "hashValue";
			var hasher = new StubHasher(hashValue);
			var fileResolver = new FileSystemResolver();

			var uri = Assembly.GetExecutingAssembly().CodeBase;
			var cssFilePath = Path.GetDirectoryName(uri) + TestUtilities.PreparePath(@"\subdirectory\output.css");
			var url = "../" + Path.GetFileName(uri) + "?test=value";
			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var expectedUrl = url + "&" + hashQueryStringKeyName + "=" + hashValue;

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
		}

		[Test]
		public void CanAppendFileHashToAbsoluteUri()
		{
			var hashQueryStringKeyName = "v";
			var hashValue = "hashValue";
			var hasher = new StubHasher(hashValue);
			var uri = Assembly.GetExecutingAssembly().CodeBase;
			var cssFilePath = Path.Combine(Path.GetDirectoryName(uri), @"output.css");
			var url = "/" + Path.GetFileName(uri);
			var pathToResolveTo = Assembly.GetExecutingAssembly().Location;
			var fileResolver = StubResolver.ForFile(pathToResolveTo);

			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var expectedUrl = url + "?" + hashQueryStringKeyName + "=" + hashValue;

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
		}

		[Test]
		public void CanAppendFileHashToAbsoluteUriWithAnExistingQueryString()
		{
			var hashQueryStringKeyName = "v";
			var hashValue = "hashValue";
			var hasher = new StubHasher(hashValue);
			var uri = Assembly.GetExecutingAssembly().CodeBase;
			var cssFilePath = Path.GetDirectoryName(uri) + @"\output.css";
			var url = "/" + Path.GetFileName(uri) + "?test=value";
			var pathToResolveTo = Assembly.GetExecutingAssembly().Location;
			var fileResolver = StubResolver.ForFile(pathToResolveTo);

			var assetsFileHasher = new CSSAssetsFileHasher(hashQueryStringKeyName, fileResolver, hasher, _pathTranslator, _httpUtility);

			var expectedUrl = url + "&" + hashQueryStringKeyName + "=" + hashValue;

			var rewrittenUrl = assetsFileHasher.AppendFileHash(cssFilePath, url);

			Assert.That(rewrittenUrl, Is.EqualTo(expectedUrl));
		}
	}
}