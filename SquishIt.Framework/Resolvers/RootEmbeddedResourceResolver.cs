using System.Diagnostics.Contracts;

namespace SquishIt.Framework.Resolvers
{
	/// <summary>
	/// 
	/// </summary>
	public class RootEmbeddedResourceResolver : EmbeddedResourceResolver
	{
		protected override string CalculateResourceName(string assemblyName, string resourceName)
		{
			return resourceName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public RootEmbeddedResourceResolver(ITempPathProvider tempPathProvider) : base(tempPathProvider)
		{
			Contract.Requires(tempPathProvider != null);
		}
	}
}