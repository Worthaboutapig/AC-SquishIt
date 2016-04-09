namespace SquishIt.AspNet
{
	using System.Web;
	using Framework.Web;

	/// <summary>
	/// Wraps the <see cref="VirtualPathUtility"/>.
	/// </summary>
	public class VirtualPathUtilityWrapper : IVirtualPathUtility
	{
		/// <summary>
		/// Converts a virtual path to an application absolute path.
		/// </summary>
		/// <param name="virtualPath">The virtual path to convert to an application-relative path.</param>
		/// <returns>The absolute path representation of the specified virtual path.</returns>
		public string ToAbsolute(string virtualPath)
		{
			return VirtualPathUtility.ToAbsolute(virtualPath);
		}
	}
}