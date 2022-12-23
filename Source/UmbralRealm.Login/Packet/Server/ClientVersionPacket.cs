using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// Unknown information that seems like it might be some version for files.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.ClientVersion)]
    public class ClientVersionPacket : IPacket
    {
        /// <summary>
        /// Client Version. Seems to be "7" from the server.
        /// </summary>
        public float Version { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutFloat(this.Version);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Version = reader.GetFloat();
        }
    }
}
