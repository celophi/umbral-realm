using BinarySerialization;
using UmbralRealm.Core.IO;

namespace UmbralRealm.Login.Packet
{
    public class WorldSelectionInfo
    {
        /// <summary>
        /// World identifier. Seems to start at '1010' and increments by tens to '1020', '1030', etc.
        /// </summary>
        [FieldOrder(0)]
        public ushort WorldId { get; set; }

        /// <summary>
        /// Server name and channel name combined with a "-" character separating them.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString WorldName { get; set; } = new();

        [FieldOrder(2)]
        public uint Unknown1 { get; set; }

        [FieldOrder(3)]
        public LengthPrefixedString Unknown2 { get; set; } = new();

        [FieldOrder(4)]
        public ushort Unknown3 { get; set; }

        [FieldOrder(5)]
        public uint Unknown4 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(6)]
        public ushort Unknown5 { get; set; }

        /// <summary>
        /// Indicates if the world is online or offline.
        /// </summary>
        [FieldOrder(7)]
        public ushort Status { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(8)]
        public ushort Unknown6 { get; set; }

        /// <summary>
        /// Number displayed in parenthesis. (I think this is population).
        /// </summary>
        [FieldOrder(9)]
        public ushort Population { get; set; }

        [FieldOrder(10)]
        public ushort Unknown7 { get; set; }

        [FieldOrder(11)]
        public ushort Unknown8 { get; set; }

        /// <summary>
        /// Description of the current channel population.
        /// </summary>
        [FieldOrder(12)]
        public ChannelCapacity Capacity { get; set; }
    }
}
