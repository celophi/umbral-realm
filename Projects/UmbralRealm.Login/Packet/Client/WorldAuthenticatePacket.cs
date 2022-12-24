using BinarySerialization;
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
        [FieldOrder(0)]
        public ushort WorldId { get; set; }

        /// <summary>
        /// Username of the account.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString Account { get; set; } = new();

        /// <summary>
        /// Encrypted or hashed password of the account.
        /// </summary>
        [FieldOrder(2)]
        public LengthPrefixedString Password { get; set; } = new();
    }
}
