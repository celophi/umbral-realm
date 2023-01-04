using System;
using System.IO;
using System.Threading.Tasks;
using BinarySerialization;
using Ralph.Crc32C;
using Zlib.Extended;
using Zlib.Extended.Enumerations;

namespace UmbralRealm.Archiver
{
    /// <summary>
    /// Provides methods for uncompressing files described by a package index.
    /// </summary>
    public class Unpacker
    {
        /// <summary>
        /// Absolute path to the package index file.
        /// </summary>
        private readonly string _indexPath;

        /// <summary>
        /// Absolute path to the destination where to write uncompressed files.
        /// </summary>
        private readonly string _outputPath;

        /// <summary>
        /// Creates an unpacker that can decompress files listed within a package index.
        /// </summary>
        /// <param name="indexPath"></param>
        /// <param name="outputPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Unpacker(string indexPath, string outputPath)
        {
            _indexPath = indexPath ?? throw new ArgumentNullException(nameof(indexPath));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
        }

        /// <summary>
        /// Runs the unpacker decompressing all files.
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Loading package index file...");

            var index = await this.LoadPackageIndex();
            Console.WriteLine("Package index deserialized!");

            Console.WriteLine($"Decompressing {index.Entries.Count} files...");
            await Parallel.ForEachAsync(index.Entries, async (entry, _) =>
            {
                try
                {
                    var filename = Path.GetFileName(_indexPath);
                    var packageId = entry.PackageId.ToString("D3");
                    var archive = _indexPath.Replace(filename, $"pkg{packageId}.pkg");

                    var compressed = new byte[entry.CompressedSize];
                    using var fs = File.OpenRead(archive);
                    fs.Position = entry.CompressedOffset;
                    await fs.ReadAsync(compressed, _);

                    var outputdata = this.DecompressData(compressed, (int)entry.DecompressedSize);

                    if (!this.IsChecksumValid(entry.Checksum, outputdata))
                    {
                        throw new InvalidDataException("Error. Checksum for the decompressed data is invalid.");
                    }

                    var absolutePath = Path.Combine(_outputPath, entry.FilePath, entry.FileName);
                    await this.WriteDecompressedFile(absolutePath, outputdata);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error. Unable to decompress file correctly.");
                    throw;
                }
            });

            sw.Stop();
            Console.WriteLine($"Finished decompressing files in {sw.Elapsed.TotalSeconds} seconds!");
        }

        /// <summary>
        /// Loads the package index file and deserializes it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        private async Task<PackageIndex> LoadPackageIndex()
        {
            var serializer = new BinarySerializer();
            var buffer = await File.ReadAllBytesAsync(_indexPath);
            var index = await serializer.DeserializeAsync<PackageIndex>(buffer);
            return index ?? throw new InvalidDataException("Package index data is invalid.");
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

            if (Compressor.Decompress(data, ref buffer) != ReturnCode.Z_STREAM_END)
            {
                throw new InvalidDataException("Error. Unable to decompress data correctly.");
            }

            return buffer;
        }

        /// <summary>
        /// Saves the decompressed data to a file on disk.
        /// </summary>
        /// <param name="path">Absolute path including file name to where the data should be saved.</param>
        /// <param name="data">Decompressed data.</param>
        /// <returns></returns>
        private async Task WriteDecompressedFile(string path, byte[] data)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory!);
            await File.WriteAllBytesAsync(path, data);
        }

        /// <summary>
        /// Performs a CRC32C checksum over a byte array and returns 'true' if the value is the expected value.
        /// </summary>
        /// <param name="expected">Expected checksum value</param>
        /// <param name="data">Data to perform the check over</param>
        /// <returns></returns>
        private bool IsChecksumValid(uint expected, byte[] data)
        {
            var crc = new Crc32C();
            crc.Update(data, 0, data.Length);
            return expected == crc.GetIntValue();
        }
    }
}
