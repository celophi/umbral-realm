using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Client
{
    /// <summary>
    /// Authentication request made to the login server.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.LoginAuthenticate)]
    public class LoginAuthenticatePacket : IPacket
    {
        /// <summary>
        /// Username of the account.
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Encrypted or hashed password of the account.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Version string of the client.
        /// </summary>
        public string ClientVersion { get; set; } = string.Empty;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutLPString(this.Account);
            writer.PutLPString(this.Password);
            writer.PutLPString(this.ClientVersion);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Account = reader.GetLPString();
            this.Password = reader.GetLPString();
            this.ClientVersion = reader.GetLPString();
        }
    }
}
