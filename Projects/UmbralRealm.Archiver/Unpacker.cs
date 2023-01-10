using System;
using System.IO;
using System.Threading.Tasks;
using Ralph.Crc32C;
using UmbralRealm.Archiver.Interfaces;

namespace UmbralRealm.Archiver
{
    /// <summary>
    /// Provides methods for uncompressing files described by a package index.
    /// </summary>
    public class Unpacker
    {
        /// <summary>
        /// Used to load and save decompressed package data.
        /// </summary>
        private readonly IPackageSystem _packageSystem;

        /// <summary>
        /// Directory path to where all packages and index file exist.
        /// </summary>
        private readonly string _packagesDirectory;

        /// <summary>
        /// Absolute path to the destination where to write uncompressed files.
        /// </summary>
        private readonly string _outputPath;

        /// <summary>
        /// Creates an unpacker that can decompress files listed within a package index.
        /// </summary>
        /// <param name="packageSystem"></param>
        /// <param name="packagesDirectory"
        /// <param name="outputPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Unpacker(IPackageSystem packageSystem, string packagesDirectory, string outputPath)
        {
            _packageSystem = packageSystem ?? throw new ArgumentNullException(nameof(packageSystem));
            _packagesDirectory = packagesDirectory ?? throw new ArgumentNullException(nameof(packagesDirectory));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
        }

        /// <summary>
        /// Runs the unpacker decompressing all files.
        /// </summary>
        /// <returns></returns>
        public async Task ExtractAll()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Loading package index file...");

            var index = await _packageSystem.LoadIndex(_packagesDirectory);
            Console.WriteLine("Package index deserialized!");

            Console.WriteLine($"Decompressing {index.Entries.Count} files...");
            await Parallel.ForEachAsync(index.Entries, async (entry, _) => await this.Extract(entry));

            sw.Stop();
            Console.WriteLine($"Finished decompressing files in {sw.Elapsed.TotalSeconds} seconds!");
        }

        /// <summary>
        /// Extracts a single package index entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public async Task Extract(PackageIndexEntry entry)
        {
            try
            {
                var data = await _packageSystem.LoadPackage(entry, _packagesDirectory);

                if (!this.IsChecksumValid(data, entry.Checksum))
                {
                    throw new InvalidDataException("Error. Checksum for the decompressed data is invalid.");
                }

                await _packageSystem.Save(entry, _outputPath, data);
            }
            catch (Exception)
            {
                Console.WriteLine("Error. Unable to decompress file correctly.");
                throw;
            }
        }

        /// <summary>
        /// Performs a CRC32C checksum over a byte array and returns 'true' if the value is the expected value.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        private bool IsChecksumValid(byte[] data, uint checksum)
        {
            var crc = new Crc32C();
            crc.Update(data, 0, data.Length);
            return checksum == crc.GetIntValue();
        }
    }
}
