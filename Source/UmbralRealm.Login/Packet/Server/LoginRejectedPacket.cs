using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// Authentication failure packet sent from the server to the client on login.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.LoginRejected)]
    public class LoginRejectedPacket : IPacket
    {
        /// <summary>
        /// Unknown, but my guess is that this is some index to the reason of the failure.
        /// </summary>
        public ushort Reason { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.Reason);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Reason = reader.GetUInt16();
        }
    }
}
