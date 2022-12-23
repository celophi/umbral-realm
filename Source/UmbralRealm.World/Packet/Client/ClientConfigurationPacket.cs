using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.World.Packet.Model;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.ClientConfiguration)]
    public class ClientConfigurationPacket : IPacket
    {
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Number of configuration entries.
        /// </summary>
        public ushort Count { get; set; }

        /// <summary>
        /// Entries found in the file.
        /// </summary>
        public List<ClientConfigurationEntry> Entries { get; set; } = new();

        /// <summary>
        /// This is probably(?) the maximum number of config hexadecimal values in the hardcoded table loaded on the client.
        /// There is a loop that goes from 0 -> 101, and that's probably what this number is related to.
        /// </summary>
        public ushort MaxValue { get; set; } = 102;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.Unknown1);
            writer.PutUInt16(this.Count);

            foreach (var entry in this.Entries ?? Enumerable.Empty<ClientConfigurationEntry>())
            {
                writer.PutUInt16((ushort)entry.Type);
                writer.PutLPString(entry.Value);
            }

            writer.PutUInt16(this.MaxValue);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Unknown1 = reader.GetUInt16();
            this.Count = reader.GetUInt16();

            for (var i = 0; i < this.Count; i++)
            {
                var entry = new ClientConfigurationEntry();
                entry.Type = (ClientConfigurationType)reader.GetUInt16();
                entry.Value = reader.GetLPString();
                this.Entries.Add(entry);
            }

            this.MaxValue = reader.GetUInt16();
        }
    }
}
