namespace SquishIt.Nancy
{
    using Web;

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
