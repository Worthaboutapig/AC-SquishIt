namespace SquishIt.Framework.Web
{
	using System;
	using System.Collections.Specialized;

	/// <summary>
	/// 
	/// </summary>
	public interface IHttpRequest
	{
		Uri Url { get; }

		NameValueCollection QueryString { get; }

		string ApplicationPath { get; }
	}
}