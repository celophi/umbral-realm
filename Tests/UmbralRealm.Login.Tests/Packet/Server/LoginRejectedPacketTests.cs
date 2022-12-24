using System.Reflection;
using System.Threading.Tasks;
using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Login.Packet;
using UmbralRealm.Login.Packet.Server;
using UmbralRealm.Testing.Utilities;
using Xunit;

namespace UmbralRealm.Login.Tests.Packet.Server
{
    public class LoginRejectedPacketTests
    {
        /// <summary>
        /// Binary dump file used for testing.
        /// </summary>
        private const string BinaryDump = "0x0004_LC_LoginRejected.bin";

        /// <summary>
        /// Expected reason
        /// </summary>
        private const ushort Reason = 2;

        /// <summary>
        /// Assert that packet model is correct and can deserialize a real packet dump.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Deserialize_BufferValid_PropertiesPopulated()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var buffer = BinaryDumpReader.LoadEmbedded(BinaryDump, assembly);

            using var reader = new BinaryStreamReader(buffer);
            var opcode = reader.GetUInt16();
            var remaining = reader.GetRemaining();

            Assert.Equal((ushort)PacketOpcode.LoginRejected, opcode);

            var serializer = new BinarySerializer();
            var packet = await serializer.DeserializeAsync<LoginRejectedPacket>(remaining);

            Assert.Equal(Reason, packet.Reason);
        }
    }
}
