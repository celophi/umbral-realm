using System.IO;
using System.Linq;
using System.Reflection;

namespace UmbralRealm.Testing.Utilities
{
    public static class BinaryDumpReader
    {
        /// <summary>
        /// Loads an embedded resource as a byte array.
        /// </summary>
        /// <param name="resource">Name of the resource.</param>
        /// <param name="assembly">Assembly containing the resource</param>
        /// <returns></returns>
        public static byte[] LoadEmbedded(string resource, Assembly assembly)
        {
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(resource));

            using var stream = assembly.GetManifestResourceStream(resourceName!);
            using var memoryStream = new MemoryStream();
            stream!.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
