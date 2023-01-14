using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// World selection information and availability sent from the server to the client.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldSelection)]
    public class WorldSelectionPacket : IPacket
    {
        /// <summary>
        /// Number of worlds that are available.
        /// </summary>
        [FieldOrder(0)]
        public ushort WorldCount { get; set; }

        /// <summary>
        /// Server and channel information for connecting to a world.
        /// </summary>
        [FieldOrder(1)]
        [FieldCount(nameof(WorldCount))]
        public List<WorldSelectionInfo> WorldSelectionInfoList = new();

        /// <summary>
        /// World ID that is highlighted by default.
        /// </summary>
        [FieldOrder(2)]
        public ushort DefaultWorldId { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(3)]
        public byte Unknown2 { get; set; }
    }
}
