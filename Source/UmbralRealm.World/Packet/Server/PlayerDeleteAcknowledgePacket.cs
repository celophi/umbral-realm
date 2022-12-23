using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.PlayerDeleteAcknowledge)]
    public class PlayerDeleteIntentPacket : IPacket
    {
        /// <summary>
        /// Unknown. Seems to be zero.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.Unknown);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Unknown = reader.GetUInt16();
        }
    }
}
