using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldLoginFailure)]
    public class WorldLoginFailurePacket : IPacket
    {
        /// <summary>
        /// Unknown. Usually 1000
        /// </summary>
        [FieldOrder(0)]
        public uint Unknown1 { get; set; }
    }
}
