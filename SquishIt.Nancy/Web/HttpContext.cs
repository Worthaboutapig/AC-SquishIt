using SquishIt.Framework.Web;

namespace SquishIt.Nancy.Web
{
	using System.Configuration;

	/// <summary>
	/// Provides access to an HTTP context.
	/// </summary>
	public class HttpContext : IHttpContext
	{
		/// <summary>
		/// Initialises the wrapper with the <see cref="HttpContext"/>, reading the debugging setting from the configuration file.
		/// </summary>
		public HttpContext()
		{
			var compilationConfigurationSection = ConfigurationManager.GetSection("system.web/compilation") as CompilationConfigurationSection;
			IsDebuggingEnabled = compilationConfigurationSection == null ? false : compilationConfigurationSection.Debug;
		}

		/// <summary>
		/// The current HTTP request.
		/// </summary>
		public IHttpRequest Request { get; }

		/// <summary>
		/// The server.
		/// </summary>
		public IServer Server { get; }

		/// <summary>
		/// Indicates whether the current HTTP request is in debug mode.
		/// </summary>
		/// <remarks>Nancy does not have such a property in its context, so we read it from the configuration file.
		/// The default value if <c>false</c>, if the configuration element <see cref="https://msdn.microsoft.com/en-gb/library/s10awwz0(v=vs.85).aspx"/> is missing.
		/// To set <see cref="IsDebuggingEnabled"/> as true, ensure that you configuration file includes the following: <code><compilation debug="true" /></code>.
		/// </remarks>
		/// <returns>
		/// <c>True</c> if the request is in debug mode, <c>false</c> otherwise.
		/// </returns>
		public bool IsDebuggingEnabled { get; }

		/// <summary>
		/// We have to create our own configuration section here, as this section lives in <see cref="System.Web"/>, which we are intentionally abstracting from!
		/// </summary>
		private class CompilationConfigurationSection : ConfigurationSection
		{
			[ConfigurationProperty("debug", DefaultValue = false)]
			public bool Debug { get; set; }
		}
	}
}