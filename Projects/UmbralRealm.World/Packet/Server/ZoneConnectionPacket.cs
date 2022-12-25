using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    /// <summary>
    /// World IP address and port information sent from the server to the client upon authenticating to the world server.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.ZoneConnection)]
    public class ZoneConnectionPacket : IPacket
    {
        /// <summary>
        /// Unknown. Seems unused?
        /// </summary>
        [FieldOrder(0)]
        public uint Unknown1 { get; set; }

        [FieldOrder(1)]
        public uint Unknown2 { get; set; }

        [FieldOrder(2)]
        public uint ZoneIPAddress { get; set; }

        [FieldOrder(3)]
        public ushort ZoneIPPort { get; set; }

        [FieldOrder(4)]
        [FieldLength(8)]
        public byte[] Unknown3 { get; set; } = new byte[8];
    }
}
