using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// Unknown information that seems like it might be some version for files.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.ClientVersion)]
    public class ClientVersionPacket : IPacket
    {
        /// <summary>
        /// Client Version. Seems to be "7" from the server.
        /// </summary>
        [FieldOrder(0)]
        public float Version { get; set; }
    }
}
