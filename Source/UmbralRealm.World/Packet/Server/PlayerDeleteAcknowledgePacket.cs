using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.PlayerDeleteAcknowledge)]
    public class PlayerDeleteIntentPacket : IPacket
    {
        /// <summary>
        /// Unknown. Seems to be zero.
        /// </summary>
        [FieldOrder(0)]
        public ushort Unknown { get; set; }
    }
}
