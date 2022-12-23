namespace UmbralRealm.Core.Network
{
    /// <summary>
    /// Configuration options for creating sockets.
    /// </summary>
    public class SocketConfiguration
    {
        /// <summary>
        /// IPv4 address of the socket.
        /// </summary>
        public string IPAddress { get; set; } = string.Empty;

        /// <summary>
        /// Port of the socket.
        /// </summary>
        public short Port { get; set; }
    }
}
