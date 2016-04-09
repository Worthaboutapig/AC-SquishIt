using System.Diagnostics.Contracts;

namespace SquishIt.Framework.Resolvers
{
	/// <summary>
	/// 
	/// </summary>
	public class StandardEmbeddedResourceResolver : EmbeddedResourceResolver
	{
		protected override string CalculateResourceName(string assemblyName, string resourceName)
		{
			return assemblyName + "." + resourceName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public StandardEmbeddedResourceResolver(ITempPathProvider tempPathProvider) : base(tempPathProvider)
		{
			Contract.Requires(tempPathProvider != null);
		}
	}
}