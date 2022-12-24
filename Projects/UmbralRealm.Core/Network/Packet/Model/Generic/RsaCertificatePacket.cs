using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Packet.Model.Generic
{
    /// <summary>
    /// Server response packet sent to the client on initial connection.
    /// This packet is sent to establish secure network transmission.
    /// </summary>
    public class RsaCertificatePacket : IPacket
    {
        /// <summary>
        /// Fixed size of bytes for this packet type.
        /// </summary>
        public const int FixedSize = 266;

        /// <summary>
        /// The RSA public key modulus used to setup network transmission.
        /// </summary>
        public byte[] Modulus { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// The RSA public key exponent used to setup network transmission.
        /// </summary>
        public byte[] Exponent { get; set; } = Array.Empty<byte>();

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutInt32(this.Modulus.Length);
            writer.PutInt32(this.Exponent.Length);
            writer.PutBytes(this.Modulus);
            writer.PutBytes(this.Exponent);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            var modulusLength = reader.GetInt32();
            var exponentLength = reader.GetInt32();

            this.Modulus = reader.GetBytes(modulusLength);
            this.Exponent = reader.GetBytes(exponentLength);
        }
    }
}
