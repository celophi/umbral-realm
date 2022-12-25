using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.UnknownTimestamps)]
    public class UnknownTimestampsPacket : IPacket
    {
        /// <summary>
        /// Timestamp that is 2 hours in the future.
        /// </summary>
        [FieldOrder(0)]
        public uint TimestampFuture { get; set; }

        [FieldOrder(1)]
        public uint Unknown { get; set; }

        /// <summary>
        /// Timestamp that is 3 hours in the past.
        /// </summary>
        [FieldOrder(2)]
        public uint TimestampOld { get; set; }
    }
}
