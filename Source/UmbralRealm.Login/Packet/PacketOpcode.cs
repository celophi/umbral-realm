using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Packet;

namespace UmbralRealm.Login.Packet
{
    /// <summary>
    /// Unique identifier for the packet.
    /// </summary>
    public enum PacketOpcode : ushort
    {
        /// <summary>
        /// Includes account, password, and client version information, sent from the client to server upon login.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.Login)]
        LoginAuthenticate = 0x0001,

        /// <summary>
        /// Includes selected world from <see cref="WorldSelection"/>, account, and password information, sent from the client to server.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.Login)]
        WorldAuthenticate = 0x0002,

        /// <summary>
        /// String that is sent upon login that is always " 7.0" as a float.
        /// However, in the client it seems to get overwritten making this packet useless.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.Login)]
        ClientVersion = 0x0003,

        /// <summary>
        /// Server response indicating that <see cref="LoginAuthenticate"/> has failed.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.Login)]
        LoginRejected = 0x0004,

        /// <summary>
        /// Includes world selection information.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.Login)]
        WorldSelection = 0x0005,

        /// <summary>
        /// Upon successful login, server sends world connection information.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.Login)]
        WorldConnection = 0x0006,
    }
}
