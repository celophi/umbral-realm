using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Zlib.Extended.Enumerations;
using Zlib.Extended.Interfaces;

namespace UmbralRealm.Archiver.Tests
{
    public class PackageSystemTests
    {
        [Fact]
        public void Constructor_FileSystemArgumentIsNull_Throws()
        {
            var compressor = new Mock<ICompressor>();
            Assert.Throws<ArgumentNullException>(() => new PackageSystem(fileSystem: null!, compressor.Object));
        }

        [Fact]
        public void Constructor_CompressorArgumentIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            Assert.Throws<ArgumentNullException>(() => new PackageSystem(fileSystem.Object, compressor: null));
        }

        [Fact]
        public async Task LoadIndex_PackageDirectoryIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => packageSystem.LoadIndex(packageDirectory: null!));
        }

        [Fact]
        public async Task LoadIndex_PackageDirectoryValid_DeserializesIndex()
        {
            var packageDirectory = "packageDirectory";
            var indexPath = Path.Combine(packageDirectory, "pkg.idx");

            var file = new Mock<IFile>();
            file.Setup(m => m.ReadAllBytesAsync(indexPath, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Array.Empty<byte>()));

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(m => m.File).Returns(file.Object);

            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            var index = await packageSystem.LoadIndex(packageDirectory);

            Assert.NotNull(index);
            file.Verify(m => m.ReadAllBytesAsync(indexPath, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Save_EntryArgumentIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => packageSystem.Save(entry: null!, string.Empty, Array.Empty<byte>()));
        }

        [Fact]
        public async Task Save_OutputDirectoryArgumentIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => packageSystem.Save(new PackageIndexEntry(), outputDirectory: null!, data: Array.Empty<byte>()));
        }

        [Fact]
        public async Task Save_UncompressedDataProvided_SavesAtEntryPath()
        {
            var directory = new Mock<IDirectory>();
            var file = new Mock<IFile>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(m => m.Directory).Returns(directory.Object);
            fileSystem.Setup(m => m.File).Returns(file.Object);

            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            var entry = new PackageIndexEntry();
            entry.FilePath = "filepath";
            entry.FileName = "filename";

            var outputDirectory = "output";

            var data = new byte[] { 0x01, 0x02, 0x03 };

            await packageSystem.Save(entry, outputDirectory, data);

            directory.Verify(m => m.CreateDirectory(It.IsAny<string>()), Times.Once);
            file.Verify(m => m.WriteAllBytesAsync(It.IsAny<string>(), data, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task LoadPackage_EntryArgumentIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => packageSystem.LoadPackage(entry: null!, string.Empty));
        }

        [Fact]
        public async Task LoadPackage_PackageDirectoryArgumentIsNull_Throws()
        {
            var fileSystem = new Mock<IFileSystem>();
            var compressor = new Mock<ICompressor>();
            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => packageSystem.LoadPackage(entry: new PackageIndexEntry(), packageDirectory: null));
        }

        [Fact]
        public async Task LoadPackage_DecompressorAbortsEarly_Throws()
        {
            var stream = new Mock<FileSystemStream>(null, "path", false);
            
            var file = new Mock<IFile>();
            file.Setup(m => m.OpenRead(It.IsAny<string>())).Returns(stream.Object);

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(m => m.File).Returns(file.Object);

            var compressor = new Mock<ICompressor>();
            compressor.Setup(m => m.Decompress(It.IsAny<byte[]>(), ref It.Ref<byte[]>.IsAny)).Returns(ReturnCode.Z_OK);

            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            var entry = new PackageIndexEntry();
            var directory = "packagedirectory";

            await Assert.ThrowsAsync<InvalidDataException>(() => packageSystem.LoadPackage(entry, directory));
        }

        [Fact]
        public async Task LoadPackage_DecompressorRunsToStreamEnd_ReturnsDecompressedData()
        {
            var stream = new Mock<FileSystemStream>(null, "path", false);

            var file = new Mock<IFile>();
            file.Setup(m => m.OpenRead(It.IsAny<string>())).Returns(stream.Object);

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(m => m.File).Returns(file.Object);

            var compressor = new Mock<ICompressor>();
            compressor.Setup(m => m.Decompress(It.IsAny<byte[]>(), ref It.Ref<byte[]>.IsAny)).Returns(ReturnCode.Z_STREAM_END);

            var packageSystem = new PackageSystem(fileSystem.Object, compressor.Object);

            var entry = new PackageIndexEntry();
            var directory = "packagedirectory";

            var decompressed = await packageSystem.LoadPackage(entry, directory);

            Assert.NotNull(decompressed);
            compressor.Verify(m => m.Decompress(It.IsAny<byte[]>(), ref It.Ref<byte[]>.IsAny), Times.Once);
        }
    }
}
