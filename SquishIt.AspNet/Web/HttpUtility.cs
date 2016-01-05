using System.Collections.Specialized;
using SquishIt.Framework.Web;

namespace SquishIt.AspNet.Web
{
    /// <summary>
    /// Wraps <see cref="HttpUtility"/> in the <see cref="IHttpUtility"/> interface
    /// </summary>
    public class HttpUtility : IHttpUtility
    {
        /// <summary>
        /// Parses a query string into a NameValueCollection using UTF8 encoding.
        /// </summary>
        /// <param name="queryString">The query string to parse.</param>
        /// <returns>A NameValueCollection of query parameters and values.</returns>
        public NameValueCollection ParseQueryString(string queryString)
        {
            return System.Web.HttpUtility.ParseQueryString(queryString);
        }
    }
}
