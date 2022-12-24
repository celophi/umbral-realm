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
    public class WorldConnectionPacketTests
    {
        /// <summary>
        /// Binary dump file used for testing.
        /// </summary>
        private const string BinaryDump = "0x0006_LC_WorldConnection.bin";

        /// <summary>
        /// Expected IP address.
        /// </summary>
        private const uint WorldIPAddress = 16777343;

        /// <summary>
        /// Expected port.
        /// </summary>
        private const ushort Port = 20001;

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

            Assert.Equal((ushort)PacketOpcode.WorldConnection, opcode);

            var serializer = new BinarySerializer();
            var packet = await serializer.DeserializeAsync<WorldConnectionPacket>(remaining);

            Assert.Equal(WorldIPAddress, packet.WorldIPAddress);
            Assert.Equal(Port, packet.WorldIPPort);
        }
    }
}
