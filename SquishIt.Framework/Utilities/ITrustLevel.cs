using System.Web;

namespace SquishIt.Framework.Utilities
{
    /// <summary>
    /// Maps the AspNet hosting permission levels to establish trust levels.
    /// </summary>
    public interface ITrustLevel
    {
        /// <summary>
        /// The current trust level.
        /// </summary>
        AspNetHostingPermissionLevel CurrentTrustLevel { get; }

        /// <summary>
        /// Whether the <see cref="CurrentTrustLevel"/> is <see cref="AspNetHostingPermissionLevel.Unrestricted"/>.
        /// </summary>
        bool IsFullTrust { get; }

        /// <summary>
        /// Whether the <see cref="CurrentTrustLevel"/> is <see cref="AspNetHostingPermissionLevel.High"/> or <see cref="AspNetHostingPermissionLevel.Unrestricted"/>.
        /// </summary>
        bool IsHighOrUnrestrictedTrust { get; }
    }
}