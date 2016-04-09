namespace SquishIt.Nancy
{
	using System;
	using Framework.Web;

	/// <summary>
	/// 
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
			throw new NotSupportedException("Remember the Nancy equivalent to VirtualPathUtility.");
		}
	}
}