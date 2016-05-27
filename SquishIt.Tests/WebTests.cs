//using System;
//using SquishIt.Framework.Web;

//namespace SquishIt.Tests
//{
//    /// <summary>
//    /// Allows injecting the necessary interfaces for different web frameworks into test classes
//    /// </summary>
//    public abstract class WebTests
//    {
//        protected readonly IHttpUtility httpUtility;

//        protected WebTests(IHttpUtility httpUtility)
//        {
//            if (httpUtility == null)
//            {
//                throw new ArgumentNullException("httpUtility");
//            }

//            this.httpUtility = httpUtility;
//        }
//    }
//}