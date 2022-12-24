using BinarySerialization;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Packet.Model.Generic
{
    /// <summary>
    /// Represents a packet that has not been identified.
    /// </summary>
    public class UnknownPacket : IPacket
    {
        /// <summary>
        /// Indicates the server type if applicable.
        /// </summary>
        [Ignore]
        public ServerType ServerType { get; set; }

        /// <summary>
        /// Indicates the origin of the packet if applicable.
        /// </summary>
        [Ignore]
        public PacketOrigin Origin { get; set; }

        /// <inheritdoc/>
        [FieldOrder(0)]
        public ushort Opcode { get; set; }

        /// <summary>
        /// Unknown data.
        /// </summary>
        [FieldOrder(1)]
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
}
