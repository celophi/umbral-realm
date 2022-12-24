using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// World IP address and port information sent from the server to the client upon authenticating to the world server.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldConnection)]
    public class WorldConnectionPacket : IPacket
    {
        /// <summary>
        /// Unknown. Seems unused?
        /// </summary>
        [FieldOrder(0)]
        public uint Unknown1 { get; set; }

        [FieldOrder(1)]
        public uint Unknown2 { get; set; }

        [FieldOrder(2)]
        public uint WorldIPAddress { get; set; }

        [FieldOrder(3)]
        public ushort WorldIPPort { get; set; }

        [FieldOrder(4)]
        [FieldLength(8)]
        public byte[] Unknown3 { get; set; } = new byte[8];
    }
}
