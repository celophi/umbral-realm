using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.PlayerDeleteConfirm)]
    public class PlayerDeleteConfirmPacket : IPacket
    {
        /// <summary>
        /// Uniquely identifies the player.
        /// </summary>
        [FieldOrder(0)]
        public uint PlayerId { get; set; }

        /// <summary>
        /// Encrypted password for confirmation.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString Password { get; set; } = new();
    }
}
