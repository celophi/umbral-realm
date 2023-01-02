using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using UmbralRealm.Core.IO;

namespace UmbralRealm.Archiver
{
    public class Unpacker
    {
        private readonly string _indexPath;

        public Unpacker(string indexPath)
        {
            _indexPath = indexPath ?? throw new ArgumentNullException(nameof(indexPath));
        }

        public async Task Run(string outputPath)
        {
            var buffer = await File.ReadAllBytesAsync(_indexPath);

            var serializer = new BinarySerializer();
            var index = await serializer.DeserializeAsync<PackageIndex>(buffer);

            if (index == null)
            {
                throw new InvalidDataException("Package index data is invalid.");
            }

            //Parallel.ForEach(index.Entries, (entry) =>
            //{
            foreach (var entry in index.Entries)
            {
                try
                {
                    var packageId = entry.PackageId.ToString("D3");
                    var archive = _indexPath.Replace("pkg.idx", $"pkg{packageId}.pkg");

                    var compressed = new byte[entry.CompressedSize];
                    using var fs = File.OpenRead(archive);
                    fs.Position = entry.CompressedOffset;
                    fs.Read(compressed, 0, compressed.Length);

                    //using var reader = new BinaryStreamReader(compressed);
                    //if (reader.GetUInt16() != 0x9C78)
                    //{
                    //    using var writer = new BinaryStreamWriter();
                    //    writer.PutUInt16(0x9C78);
                    //    writer.PutBytes(compressed);
                    //    compressed = writer.ToArray();
                    //}

                    using var outputstream = new MemoryStream();
                    using var ms = new MemoryStream(compressed);
                    using var inflater = new InflaterInputStream(ms);
                    inflater.CopyTo(outputstream);

                    //else
                    //{
                    //    using var ds = new DeflateStream(ms, CompressionMode.Decompress);
                    //    ds.CopyTo(outputstream);
                    //}

                    var outputFile = Path.Combine(outputPath, entry.FilePath.Trim('\0'));
                    var fullPath = Path.Combine(outputFile, entry.FileName.Trim('\0'));
                    var fileDirectory = Path.GetDirectoryName(fullPath);
                    Directory.CreateDirectory(fileDirectory!);

                    File.WriteAllBytes(fullPath, outputstream.ToArray());
                }
                catch (Exception ex)
                {
                    var a = ex;
                }
            }

                
            //});







            //var compressed = new byte[108769];

            //using (var fs = File.OpenRead("pkg001.pkg"))
            //{
            //    fs.ReadByte();
            //    fs.ReadByte();
            //    fs.Read(compressed, 0, compressed.Length);
            //}

            //using (var ms = new MemoryStream(compressed))
            //using (var ms2 = new MemoryStream())
            //using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            //{
            //    ds.CopyTo(ms2);


            //    File.WriteAllBytes("output.bin", ms2.ToArray());
            //}





        }
    }
}
