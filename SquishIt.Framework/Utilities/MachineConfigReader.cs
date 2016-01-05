using System.Configuration;

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

	/// <summary>
	/// Reads values from the machine.config
	/// </summary>
	class MachineConfigReader : IMachineConfigReader
	{
		/// <summary>
		/// Whether the machine.config is not configured for retail deployment
		/// </summary>
		public bool IsNotRetailDeployment
		{
			get
			{
				//check retail setting in machine.config
				//Thanks Dave Ward! http://www.encosia.com
				var machineConfig = ConfigurationManager.OpenMachineConfiguration();
				var systemWebSection = machineConfig.GetSectionGroup("system.web");

				if (systemWebSection == null)
				{
					return true;
				}

				var deploymentSection = (DeploymentSection)systemWebSection.Sections["deployment"];

				return !deploymentSection.Retail;
			}
		}

		/// <summary>
		/// Extracts the value of 'Retail' from the config file, without the System.Web dependency
		/// </summary>
		private class DeploymentSection : ConfigurationSection
		{
			/// <summary>
			/// Gets or sets a value that specifies whether Web applications on the computer are deployed in retail mode.
			/// </summary>
			/// <returns>
			/// true if Web applications are deployed in retail mode; otherwise, false. The default is false.
			/// </returns>
			[ConfigurationProperty("retail", DefaultValue = false)]
			public bool Retail { get; set; }
		}
	}
}