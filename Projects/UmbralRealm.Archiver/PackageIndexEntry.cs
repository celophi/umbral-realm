using BinarySerialization;

namespace UmbralRealm.Archiver
{
    /// <summary>
    /// Metadata in the package index describing a section of data in a package archive.
    /// </summary>
    public class PackageIndexEntry
    {
        /// <summary>
        /// Uniquely identifies the entry in the package index file.
        /// </summary>
        [FieldOrder(0)]
        public uint EntryId { get; set; }

        /// <summary>
        /// Offset into the package where the compressed data for this entry starts.
        /// </summary>
        [FieldOrder(1)]
        public uint CompressedOffset { get; set; }

        /// <summary>
        /// Offset into the package index where this entry is located.
        /// </summary>
        [FieldOrder(2)]
        public uint EntryOffset { get; set; }

        /// <summary>
        /// Size in bytes of the compressed data region.
        /// </summary>
        [FieldOrder(3)]
        public uint CompressedSize { get; set; }

        /// <summary>
        /// CRC32C value to ensure that data is decompressed correctly.
        /// </summary>
        [FieldOrder(4)]
        public uint Checksum { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(5)]
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(6)]
        public ulong Flags { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(7)]
        public ulong TimeUnknown1 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(8)]
        public ulong TimeUnknown2 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(9)]
        public ulong TimeUnknown3 { get; set; }

        /// <summary>
        /// Size in bytes of the decompressed data region.
        /// </summary>
        [FieldOrder(10)]
        public uint DecompressedSize { get; set; }

        /// <summary>
        /// Name of the data region.
        /// </summary>
        [FieldOrder(11)]
        [FieldLength(260)]
        [SerializeAs(SerializedType.TerminatedString)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the directory where the file should reside.
        /// </summary>
        [FieldOrder(12)]
        [FieldLength(260)]
        [SerializeAs(SerializedType.TerminatedString)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(13)]
        public uint Unknown2 { get; set; }

        /// <summary>
        /// Identifier of the package where this data is applicable.
        /// </summary>
        [FieldOrder(14)]
        public uint PackageId { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        [FieldOrder(15)]
        public uint Unknown3 { get; set; }
    }
}
