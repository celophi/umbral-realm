using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Client
{
    /// <summary>
    /// Packet sent from the client to the server including authentication information and the selected world to connect to.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldAuthenticate)]
    public class WorldAuthenticatePacket : IPacket
    {
        /// <summary>
        /// World identifier.
        /// </summary>
        public ushort WorldId { get; set; }

        /// <summary>
        /// Username of the account.
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Encrypted or hashed password of the account.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.WorldId);
            writer.PutLPString(this.Account);
            writer.PutLPString(this.Password);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.WorldId = reader.GetUInt16();
            this.Account = reader.GetLPString();
            this.Password = reader.GetLPString();
        }
    }
}
