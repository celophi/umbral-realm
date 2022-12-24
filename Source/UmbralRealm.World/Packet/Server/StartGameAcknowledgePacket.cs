using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.StartGameAcknowledge)]
    public class StartGameAcknowledge : IPacket
    {
        /// <summary>
        /// Timestamp of the login time.
        /// </summary>
        [FieldOrder(0)]
        public uint Timestamp { get; set; }
    }
}
