using System;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Tests.Helpers;

namespace SquishIt.Tests
{
	public abstract class CoffeeScriptTests
	{
		private readonly Func<JavaScriptBundleFactory> _javaScriptBundleFactoryCreator;

		protected CoffeeScriptTests(Func<JavaScriptBundleFactory> javaScriptBundleFactoryCreator)
		{
			_javaScriptBundleFactoryCreator = javaScriptBundleFactoryCreator;
		}

		[TestCase(typeof(MsIeCoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
		[TestCase(typeof(CoffeeScript.CoffeeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
		public void CanBundleJavascriptWithArbitraryCoffeeScript(Type preprocessorType)
		{
			var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
			Assert.NotNull(preprocessor);

			var coffee = "alert 'test' ";

			var javaScriptBundleFactory = _javaScriptBundleFactoryCreator();

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

			var tag = _javaScriptBundleFactoryCreator()
				.WithDebuggingEnabled(true)
				.Create()
				.WithPreprocessor(preprocessor)
				.AddString(coffee, ".coffee")
				.Render("~/brewed.js");

			Assert.AreEqual("<script type=\"text/javascript\">(function() {\n  alert('test');\n\n}).call(this);\n</script>\n", TestUtilities.NormalizeLineEndings(tag));
		}
	}
}