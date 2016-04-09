namespace SquishIt.Framework.Web
{
	/// <summary>
	/// Abstracts the current web context.
	/// </summary>
	public interface IHttpContext
	{
		/// <summary>
		/// The current HTTP request.
		/// </summary>
		IHttpRequest Request { get; }

		/// <summary>
		/// The server.
		/// </summary>
		IServer Server { get; }

		/// <summary>
		/// Indicates whether the current HTTP request is in debug mode.
		/// </summary>
		/// <returns>
		/// <c>True</c> if the request is in debug mode, <c>false</c> otherwise.
		/// </returns>
		bool IsDebuggingEnabled { get; }
	}
}