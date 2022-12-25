using BinarySerialization;
using UmbralRealm.Core.Network.Packet;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldLoginAcknowledge)]
    public class WorldLoginAcknowledgePacket
    {
        /// <summary>
        /// This does not seem used.
        /// </summary>
        [FieldOrder(0)]
        public uint Unused { get; set; }
    }
}
