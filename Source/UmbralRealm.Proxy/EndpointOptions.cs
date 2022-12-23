namespace UmbralRealm.Proxy
{
    /// <summary>
    /// Options for configuring an endpoint.
    /// </summary>
    public class EndpointOptions
    {
        /// <summary>
        /// Section identifier in configuration.
        /// </summary>
        public const string RemoteLoginEndpoint = "RemoteLoginEndpoint";

        /// <summary>
        /// Section identifier in configuration.
        /// </summary>
        public const string LocalLoginEndpoint = "LocalLoginEndpoint";

        /// <summary>
        /// Section identifier in configuration.
        /// </summary>
        public const string LocalWorldEndpoint = "LocalWorldEndpoint";

        /// <summary>
        /// Section identifier in configuration.
        /// </summary>
        public const string LocalZoneEndpoint = "LocalZoneEndpoint";

        /// <summary>
        /// Address to use for establishing a connection.
        /// </summary>
        public string IPAddress { get; set; } = string.Empty;

        /// <summary>
        /// Port to use for establishing a connection.
        /// </summary>
        public int Port { get; set; }
    }
}
