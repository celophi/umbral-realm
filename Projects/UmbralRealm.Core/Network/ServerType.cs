namespace UmbralRealm.Core.Network
{
    /// <summary>
    /// Represents the type of server and the kind of use cases it servers.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// Authentication portal for verifying an account.
        /// </summary>
        Login,

        /// <summary>
        /// Select after properly authenticating through the login server.
        /// </summary>
        World,

        /// <summary>
        /// Unknown.
        /// </summary>
        Zone,
    }
}
