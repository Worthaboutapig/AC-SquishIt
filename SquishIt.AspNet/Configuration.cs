using SquishIt.AspNet.Web;

namespace SquishIt.AspNet
{
    /// <summary>
    /// Extends the framework configuration with the System.Web-specific configuration data.
    /// </summary>
    public class Configuration : Framework.Configuration
    {
        /// <summary>
        /// Initialise with the web framework defaults
        /// </summary>
        public Configuration()
        {
            UseHttpUtility(new HttpUtility());
        }
    }
}
