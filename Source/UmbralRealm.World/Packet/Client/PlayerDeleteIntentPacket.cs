using UmbralRealm.Core.IO;
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
        public uint PlayerId { get; set; }

        /// <summary>
        /// Unknown. Seems to be zero.
        /// </summary>
        public byte Unknown { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.PlayerId);
            writer.PutByte(this.Unknown);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.PlayerId = reader.GetUInt32();
            this.Unknown = reader.GetByte();
        }
    }
}
