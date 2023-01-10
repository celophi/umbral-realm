using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using BinarySerialization;
using UmbralRealm.Archiver.Interfaces;
using Zlib.Extended.Enumerations;
using Zlib.Extended.Interfaces;

namespace UmbralRealm.Archiver
{
    public class PackageSystem : IPackageSystem
    {
        /// <summary>
        /// Used to access files from the file system.
        /// </summary>
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Used for inflating the compressed data.
        /// </summary>
        private readonly ICompressor _compressor;

        /// <summary>
        /// Creates a system for managing a <see cref="PackageIndexEntry"/>.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="compressor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PackageSystem(IFileSystem fileSystem, ICompressor compressor)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _compressor = compressor ?? throw new ArgumentNullException(nameof(compressor));
        }

        /// <inheritdoc/>
        public async Task<PackageIndex> LoadIndex(string packageDirectory)
        {
            ArgumentNullException.ThrowIfNull(packageDirectory);

            var path = Path.Combine(packageDirectory, "pkg.idx");
            var serializer = new BinarySerializer();
            var buffer = await _fileSystem.File.ReadAllBytesAsync(path);
            return await serializer.DeserializeAsync<PackageIndex>(buffer);
        }

        /// <inheritdoc/>
        public async Task<byte[]> LoadPackage(PackageIndexEntry entry, string packageDirectory)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(packageDirectory);

            var packageId = entry.PackageId.ToString("D3");
            var archive = Path.Combine(packageDirectory, $"pkg{packageId}.pkg");

            var compressed = new byte[entry.CompressedSize];
            using var fs = _fileSystem.File.OpenRead(archive);
            fs.Position = entry.CompressedOffset;
            await fs.ReadAsync(compressed);

            return this.DecompressData(compressed, (int)entry.DecompressedSize);
        }

        /// <inheritdoc/>
        public async Task Save(PackageIndexEntry entry, string outputDirectory, byte[] data)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var absolutePath = Path.Combine(outputDirectory, entry.FilePath, entry.FileName);
            var directory = Path.GetDirectoryName(absolutePath);
            _fileSystem.Directory.CreateDirectory(directory!);
            await _fileSystem.File.WriteAllBytesAsync(absolutePath, data);
        }

        /// <summary>
        /// Decompresses a byte array.
        /// </summary>
        /// <param name="data">Compressed data.</param>
        /// <param name="size">Expected size of the decompressed data.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        private byte[] DecompressData(byte[] data, int size)
        {
            var buffer = new byte[size];

            if (_compressor.Decompress(data, ref buffer) != ReturnCode.Z_STREAM_END)
            {
                throw new InvalidDataException("Error. Unable to decompress data correctly.");
            }

            return buffer;
        }
    }
}
