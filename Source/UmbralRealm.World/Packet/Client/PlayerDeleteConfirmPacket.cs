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
        public uint PlayerId { get; set; }

        /// <summary>
        /// Encrypted password for confirmation.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.PlayerId);
            writer.PutLPString(this.Password);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.PlayerId = reader.GetUInt32();
            this.Password = reader.GetLPString();
        }
    }
}
