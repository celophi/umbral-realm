using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Player;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.CreatePlayer)]
    public class CreatePlayerPacket : IPacket
    {
        [FieldOrder(0)]
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Represents the class/job of the player.
        /// </summary>
        [FieldOrder(1)]
        public PlayerClass PlayerClass { get; set; }

        /// <summary>
        /// Usually 1.
        /// </summary>
        [FieldOrder(2)]
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Usually 0xFF 0xFF
        /// </summary>
        [FieldOrder(3)]
        public ushort Unknown4 { get; set; }

        /// <summary>
        /// Voice of the player.
        /// </summary>
        [FieldOrder(4)]
        public PlayerVoice Voice { get; set; }

        [FieldOrder(5)]
        public uint Unknown5 { get; set; }

        // Appearance start

        [FieldOrder(6)]
        [FieldLength(23)]
        public byte[] Unknown6 { get; set; } = new byte[23];

        [FieldOrder(7)]
        public PlayerAppearance Appearance { get; set; }
    }
}
