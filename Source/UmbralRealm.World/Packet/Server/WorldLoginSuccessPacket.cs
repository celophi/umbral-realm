using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldLoginSuccess)]
    public class WorldLoginSuccessPacket : IPacket
    {
        /// <summary>
        /// Unknown. Usually zero.
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.Unknown1);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Unknown1 = reader.GetUInt32();
        }
    }
}
