namespace UmbralRealm.Login.Service
{
    /// <summary>
    /// Options for configuring an endpoint.
    /// </summary>
    public class EndpointOptions
    {
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
