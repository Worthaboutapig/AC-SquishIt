namespace SquishIt.Framework.Utilities
{
	/// <summary>
	/// Maps properties on the machine.config file
	/// </summary>
	public interface IMachineConfigReader
	{
		/// <summary>
		/// Whether the machine.config is not configured for retail deployment
		/// </summary>
		bool IsNotRetailDeployment { get; }
	}
}