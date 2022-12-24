using BinarySerialization;
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
        [FieldOrder(0)]
        public LengthPrefixedString Account { get; set; } = new();

        /// <summary>
        /// Encrypted or hashed password of the account.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString Password { get; set; } = new();

        /// <summary>
        /// Version string of the client.
        /// </summary>
        [FieldOrder(2)]
        public LengthPrefixedString ClientVersion { get; set; } = new();
    }
}
