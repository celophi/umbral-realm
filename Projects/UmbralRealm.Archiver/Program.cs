using System;
using System.IO;
using System.IO.Abstractions;
using CommandLine;
using Zlib.Extended;

namespace UmbralRealm.Archiver
{
    internal class Program
    {
        private const string _packageIndexFileName = "pkg.idx";

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);

            if (!Program.HandleOptions(result.Value))
            {
                Console.ReadLine();
                return;
            }

            var unpacker = new Unpacker(new PackageSystem(new FileSystem(), new Compressor()), result.Value.Source, result.Value.Destination);
            unpacker.ExtractAll().Wait();

            Console.WriteLine("Press any key to exit the application.");
            Console.ReadLine();
        }

        /// <summary>
        /// Validates options and returns true if successful.
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        private static bool HandleOptions(Options opts)
        {
            if (!File.Exists(Path.Combine(opts.Source, _packageIndexFileName)))
            {
                Console.WriteLine("Error. Package archives were not found at the specified source path.");
                return false;
            }

            try
            {
                Directory.CreateDirectory(opts.Destination);
            }
            catch (Exception)
            {
                Console.WriteLine("Error. Destination directory is not a valid path.");
                return false;
            }

            return true;
        }
    }
}
