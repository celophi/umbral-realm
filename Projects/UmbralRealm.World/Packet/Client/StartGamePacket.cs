using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.StartGame)]
    public class StartGamePacket : IPacket
    {
        /// <summary>
        /// Uniquely identifies the player.
        /// </summary>
        [FieldOrder(0)]
        public uint PlayerId { get; set; }

        /// <summary>
        /// The unique (MAC) address of the client's network device.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString NetworkHardwareAddress { get; set; } = new();

        /// <summary>
        /// This value appears to be unchanging. It is always 0x01.
        /// </summary>
        [FieldOrder(2)]
        public byte Unknown { get; set; } = 1;
    }
}
