using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldAuthenticate)]
    public class WorldAuthenticatePacket : IPacket
    {
        [FieldOrder(0)]
        public uint Unknown1 { get; set; }

        [FieldOrder(1)]
        public uint Unknown2 { get; set; }

        /// <summary>
        /// IP address of the world the client is connecting to.
        /// </summary>
        [FieldOrder(2)]
        public uint WorldIPAddress { get; set; }

        /// <summary>
        /// Port of the world the client is connecting to.
        /// </summary>
        [FieldOrder(3)]
        public ushort WorldIPPort { get; set; }

        [FieldOrder(4)]
        public uint Unknown3 { get; set; }

        [FieldOrder(5)]
        public uint Unknown4 { get; set; }

        /// <summary>
        /// Hardware address of client's network device.
        /// </summary>
        [FieldOrder(6)]
        public LengthPrefixedString NetworkHardwareAddress { get; set; } = new();
    }
}
