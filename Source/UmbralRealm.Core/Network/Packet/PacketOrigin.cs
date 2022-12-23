namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Represents the sender or creator of the packet.
    /// </summary>
    public enum PacketOrigin
    {
        /// <summary>
        /// Player game instance.
        /// </summary>
        Client,

        /// <summary>
        /// Remote host instance.
        /// </summary>
        Server,
    }
}
