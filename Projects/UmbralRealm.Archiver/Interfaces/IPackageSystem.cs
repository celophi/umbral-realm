using System.Threading.Tasks;

namespace UmbralRealm.Archiver.Interfaces
{
    public interface IPackageSystem
    {
        /// <summary>
        /// Gets the package index file and deserializes it.
        /// </summary>
        /// <param name="packageDirectory"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        Task<PackageIndex> LoadIndex(string packageDirectory);

        /// <summary>
        /// Loads a package entry into memory and decompresses it.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="packageDirectory"></param>
        /// <returns></returns>
        Task<byte[]> LoadPackage(PackageIndexEntry entry, string packageDirectory);

        /// <summary>
        /// Saves the decompressed data to a file on disk.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="outputDirectory">Output directory to write data to.</param>
        /// <param name="data">Data to save. This should be decompressed data.</param>
        /// <returns></returns>
        Task Save(PackageIndexEntry entry, string outputDirectory, byte[] data);
    }
}
