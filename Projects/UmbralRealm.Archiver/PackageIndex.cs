using System;
using System.Collections.Generic;
using BinarySerialization;

namespace UmbralRealm.Archiver
{
    /// <summary>
    /// Represents a directory of metadata to describe compressed files in archives.
    /// </summary>
    public class PackageIndex
    {
        /// <summary>
        /// Header section of all empty bytes.
        /// </summary>
        [FieldOrder(0)]
        [FieldLength(260)]
        public byte[] Padding { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Package version string.
        /// </summary>
        [FieldOrder(1)]
        [FieldLength(32)]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// List of entries describing archived files.
        /// </summary>
        [FieldOrder(2)]
        public List<PackageIndexEntry> Entries { get; set; } = new();
    }
}
