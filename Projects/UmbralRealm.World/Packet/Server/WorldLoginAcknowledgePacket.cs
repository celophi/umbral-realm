using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldLoginAcknowledge)]
    public class WorldLoginAcknowledgePacket : IPacket
    {
        /// <summary>
        /// This does not seem used.
        /// </summary>
        [FieldOrder(0)]
        public uint Unused { get; set; }
    }
}
