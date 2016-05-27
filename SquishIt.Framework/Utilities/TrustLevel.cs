using System.Web;

namespace SquishIt.Framework.Utilities
{
    public class TrustLevel : ITrustLevel
    {
        public AspNetHostingPermissionLevel CurrentTrustLevel
        {
            get
            {
                if (_trustLevel == null)
                {
                    var lastTrustedLevel = AspNetHostingPermissionLevel.None;

                    foreach (var level in AspNetHostingPermissionLevels)
                    {
                        try
                        {
                            new AspNetHostingPermission(level).Demand();
                            lastTrustedLevel = level;
                        }
                        catch (System.Security.SecurityException)
                        {
                            break;
                        }
                    }

                    _trustLevel = lastTrustedLevel;
                }

                return _trustLevel.Value;
            }
        }

        private AspNetHostingPermissionLevel? _trustLevel;
        private static readonly AspNetHostingPermissionLevel[] AspNetHostingPermissionLevels = {
                                                              AspNetHostingPermissionLevel.Minimal,
                                                              AspNetHostingPermissionLevel.Low,
                                                              AspNetHostingPermissionLevel.Medium,
                                                              AspNetHostingPermissionLevel.High,
                                                              AspNetHostingPermissionLevel.Unrestricted
                                                          };
        public bool IsFullTrust
        {
            get
            {
                return CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted;
            }
        }

        public bool IsHighOrUnrestrictedTrust
        {
            get
            {
                return CurrentTrustLevel == AspNetHostingPermissionLevel.High || IsFullTrust;
            }
        }
    }
}
