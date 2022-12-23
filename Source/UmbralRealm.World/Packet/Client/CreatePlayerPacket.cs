using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Player;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.CreatePlayer)]
    public class CreatePlayerPacket : IPacket
    {
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Represents the class/job of the player.
        /// </summary>
        public PlayerClass PlayerClass { get; set; }

        /// <summary>
        /// Usually 1.
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Usually 0xFF 0xFF
        /// </summary>
        public ushort Unknown4 { get; set; }

        /// <summary>
        /// Voice of the player.
        /// </summary>
        public PlayerVoice Voice { get; set; }

        public uint Unknown5 { get; set; }

        // Appearance start

        public byte[] Unknown6 { get; set; } = new byte[23];

        public PlayerAppearance Appearance { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutByte(this.Unknown1);
            writer.PutByte((byte)this.PlayerClass);
            writer.PutByte(this.Unknown2);
            writer.PutUInt16(this.Unknown4);
            writer.PutByte((byte)this.Voice);
            writer.PutUInt32(this.Unknown5);
            writer.PutBytes(this.Unknown6);
            writer.PutUInt64((ulong)this.Appearance);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Unknown1 = reader.GetByte();
            this.PlayerClass = (PlayerClass)reader.GetByte();
            this.Unknown2 = reader.GetByte();
            this.Unknown4 = reader.GetUInt16();
            this.Voice = (PlayerVoice)reader.GetByte();
            this.Unknown5 = reader.GetUInt32();
            this.Unknown6 = reader.GetBytes(this.Unknown6.Length);
            this.Appearance = (PlayerAppearance)reader.GetUInt64();
        }
    }
}
