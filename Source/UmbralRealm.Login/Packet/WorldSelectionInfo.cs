using UmbralRealm.Core.IO;
using UmbralRealm.Core.IO.Interfaces;

namespace UmbralRealm.Login.Packet
{
    public class WorldSelectionInfo : IBinarySerializable
    {
        /// <summary>
        /// World identifier. Seems to start at '1010' and increments by tens to '1020', '1030', etc.
        /// </summary>
        public ushort WorldId { get; set; }

        /// <summary>
        /// Server name and channel name combined with a "-" character separating them.
        /// </summary>
        public string WorldName { get; set; } = string.Empty;

        public uint Unknown1 { get; set; }

        public string Unknown2 { get; set; } = string.Empty;

        public ushort Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public ushort Unknown5 { get; set; }

        /// <summary>
        /// Indicates if the world is online or offline.
        /// </summary>
        public ushort Status { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public ushort Unknown6 { get; set; }

        /// <summary>
        /// Number displayed in parenthesis. (I think this is population).
        /// </summary>
        public ushort Population { get; set; }

        public ushort Unknown7 { get; set; }

        public ushort Unknown8 { get; set; }

        /// <summary>
        /// Description of the current channel population.
        /// </summary>
        public ChannelCapacity Capacity { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.WorldId);
            writer.PutLPString(this.WorldName);
            writer.PutUInt32(this.Unknown1);
            writer.PutLPString(this.Unknown2);
            writer.PutUInt16(this.Unknown3);
            writer.PutUInt32(this.Unknown4);
            writer.PutUInt16(this.Unknown5);
            writer.PutUInt16(this.Status);
            writer.PutUInt16(this.Unknown6);
            writer.PutUInt16(this.Population);
            writer.PutUInt16(this.Unknown7);
            writer.PutUInt16(this.Unknown8);
            writer.PutUInt16((ushort)this.Capacity);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.WorldId = reader.GetUInt16();
            this.WorldName = reader.GetLPString();
            this.Unknown1 = reader.GetUInt32();
            this.Unknown2 = reader.GetLPString();
            this.Unknown3 = reader.GetUInt16();
            this.Unknown4 = reader.GetUInt32();
            this.Unknown5 = reader.GetUInt16();
            this.Status = reader.GetUInt16();
            this.Unknown6 = reader.GetUInt16();
            this.Population = reader.GetUInt16();
            this.Unknown7 = reader.GetUInt16();
            this.Unknown8 = reader.GetUInt16();
            this.Capacity = (ChannelCapacity)reader.GetUInt16();
        }
    }
}
