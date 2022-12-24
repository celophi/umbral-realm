using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.PlayerDeleteIntent)]
    public class PlayerDeleteIntentPacket : IPacket
    {
        /// <summary>
        /// Uniquely identifies the player.
        /// </summary>
        [FieldOrder(0)]
        public uint PlayerId { get; set; }

        /// <summary>
        /// Unknown. Seems to be zero.
        /// </summary>
        [FieldOrder(1)]
        public byte Unknown { get; set; }
    }
}
