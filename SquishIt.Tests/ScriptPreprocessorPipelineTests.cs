//using System;
//using System.Linq;
//using NUnit.Framework;
//using SquishIt.Framework;
//using SquishIt.Framework.Base;

//using SquishIt.Tests.Helpers;
//using SquishIt.Tests.Stubs;

//namespace SquishIt.Tests
//{
//	public abstract class ScriptPreprocessorPipelineTests
//	{
//		private readonly Func<JavaScriptBundleFactory> _javaScriptBundleFactoryCreator;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
//		/// </summary>
//		protected ScriptPreprocessorPipelineTests(Func<JavaScriptBundleFactory> javaScriptBundleFactoryCreator)
//		{
//			_javaScriptBundleFactoryCreator = javaScriptBundleFactoryCreator;
//		}

//		[Test]
//		public void Js_Style_Then_Global()
//		{
//			var scriptPreprocessor = new StubScriptPreprocessor();
//			var globalPreprocessor = new StubGlobalPreprocessor();

//			using (new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
//			using (new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
//			{
//				var javaScriptBundle = _javaScriptBundleFactoryCreator()
//					.WithHasher(new StubHasher("hash"))
//					.WithDebuggingEnabled(false)
//					.WithContents("start")
//					.Create();

//				var tag = javaScriptBundle
//					.Add("~/js/test.global.script")
//					.Render("~/js/output.js");

//				var contents = _javaScriptBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

//				Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
//				Assert.AreEqual("globey;\n", contents);

//				Assert.AreEqual("start", scriptPreprocessor.CalledWith);
//				Assert.AreEqual("scripty", globalPreprocessor.CalledWith);
//			}
//		}

//		[Test]
//		public void Js_Global_Then_Style()
//		{
//			var scriptPreprocessor = new StubScriptPreprocessor();
//			var globalPreprocessor = new StubGlobalPreprocessor();

//			using (new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
//			using (new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
//			{
//				var javaScriptBundle = _javaScriptBundleFactoryCreator()
//					.WithHasher(new StubHasher("hash"))
//					.WithDebuggingEnabled(false)
//					.WithContents("start")
//					.Create();

//				var tag = javaScriptBundle
//					.Add("~/js/test.script.global")
//					.Render("~/js/output.js");

//				var contents = _javaScriptBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

//				Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
//				Assert.AreEqual("scripty;\n", contents);

//				Assert.AreEqual("globey", scriptPreprocessor.CalledWith);
//				Assert.AreEqual("start", globalPreprocessor.CalledWith);
//			}
//		}

//		[Test]
//		public void Js_Stops_At_First_Extension_With_No_Defined_Preprocessor()
//		{
//			var scriptPreprocessor = new StubScriptPreprocessor();
//			var globalPreprocessor = new StubGlobalPreprocessor();

//			using (new ScriptPreprocessorScope<StubScriptPreprocessor>(scriptPreprocessor))
//			using (new GlobalPreprocessorScope<StubGlobalPreprocessor>(globalPreprocessor))
//			{
//				var javaScriptBundle = _javaScriptBundleFactoryCreator()
//					.WithHasher(new StubHasher("hash"))
//					.WithDebuggingEnabled(false)
//					.WithContents("start")
//					.Create();

//				var tag = javaScriptBundle
//					.Add("~/js/test.script.fake.global.bogus")
//					.Render("~/js/output.js");

//				var contents = _javaScriptBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

//				Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
//				Assert.AreEqual("start;\n", contents);

//				Assert.Null(scriptPreprocessor.CalledWith);
//				Assert.Null(globalPreprocessor.CalledWith);
//			}
//		}

//		[Test]
//		public void WithPreprocessor_Uses_Instance_Preprocessors()
//		{
//			var scriptPreprocessor = new StubScriptPreprocessor();
//			var globalPreprocessor = new StubGlobalPreprocessor();

//			var javaScriptBundle = _javaScriptBundleFactoryCreator()
//				.WithHasher(new StubHasher("hash"))
//				.WithDebuggingEnabled(false)
//				.WithContents("start")
//				.Create();

//			var tag = javaScriptBundle
//				.WithPreprocessor(scriptPreprocessor)
//				.WithPreprocessor(globalPreprocessor)
//				.Add("~/js/test.script.global")
//				.Render("~/js/output.js");

//			var contents = _javaScriptBundleFactoryCreator().FileWriterFactory.Files[TestUtilities.PrepareRelativePath(@"js\output.js")];

//			Assert.AreEqual("<script type=\"text/javascript\" src=\"js/output.js?r=hash\"></script>", tag);
//			Assert.AreEqual("scripty;\n", contents);

//			Assert.AreEqual("globey", scriptPreprocessor.CalledWith);
//			Assert.AreEqual("start", globalPreprocessor.CalledWith);

//			Assert.IsEmpty(Bundle.Preprocessors.Where(x => !(x is NullPreprocessor)));
//		}
//	}
//}