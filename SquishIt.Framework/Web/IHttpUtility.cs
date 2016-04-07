using System.Collections.Specialized;

namespace SquishIt.Framework.Web
{
    /// <summary>
    /// Provides methods for encoding and decoding URLs when processing Web requests.
    /// </summary>
    public interface IHttpUtility
    {
        /// <summary>
        /// Parses a query string into a NameValueCollection using UTF8 encoding.
        /// </summary>
        /// <param name="queryString">The query string to parse.</param>
        /// <returns>A NameValueCollection of query parameters and values.</returns>
        NameValueCollection ParseQueryString(string queryString);
    }
}