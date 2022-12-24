using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.Ping)]
    public class PingPacket : IPacket
    {
        /// <summary>
        /// Account name.
        /// </summary>
        [FieldOrder(0)]
        public LengthPrefixedString Account { get; set; } = new();

        /// <summary>
        /// Name of the world.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString WorldName { get; set; } = new();

        /// <summary>
        /// All zeros.
        /// </summary>
        [FieldOrder(2)]
        [FieldLength(6)]
        public byte[] Unknown1 { get; set; } = new byte[6];

        /// <summary>
        /// Client or server version string?
        /// </summary>
        [FieldOrder(3)]
        public LengthPrefixedString Version { get; set; } = new();

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(4)]
        public uint Unknown2 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(5)]
        [FieldLength(6)]
        public byte[] Unknown3 { get; set; } = new byte[6];

        /// <summary>
        /// Seems like it is supposed to be something like "Windows7-64bit".
        /// </summary>
        [FieldOrder(6)]
        public LengthPrefixedString WindowsVersion { get; set; } = new();

        /// <summary>
        /// ID of the world connected to.
        /// </summary>
        [FieldOrder(7)]
        public ushort WorldId { get; set; }
    }
}
