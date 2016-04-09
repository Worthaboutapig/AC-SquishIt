namespace SquishIt.Framework.Web
{
	/// <summary>
	/// Abstracts utility methods for common virtual path operations.
	/// </summary>
	public interface IVirtualPathUtility
	{
		/// <summary>
		/// Converts a virtual path to an application absolute path.
		/// </summary>
		/// <param name="virtualPath">The virtual path to convert to an application-relative path.</param>
		/// <returns>The absolute path representation of the specified virtual path.</returns>
		string ToAbsolute(string virtualPath);
	}
}
