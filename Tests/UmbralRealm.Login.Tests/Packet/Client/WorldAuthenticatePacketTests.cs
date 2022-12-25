using System.Reflection;
using System.Threading.Tasks;
using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Login.Packet;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Testing.Utilities;
using Xunit;

namespace UmbralRealm.Login.Tests.Packet.Client
{
    public class WorldAuthenticatePacketTests
    {
        /// <summary>
        /// Binary dump file used for testing.
        /// </summary>
        private const string BinaryDump = "0x0002_CL_WorldAuthenticate.bin";

        /// <summary>
        /// Expected world ID.
        /// </summary>
        private const ushort WorldId = 1010;

        /// <summary>
        /// Expected account string.
        /// </summary>
        private const string Account = "MyAccount";

        /// <summary>
        /// Expected password string.
        /// </summary>
        private const string Password = "48503dfd58720bd5ff35c102065a52d7";

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

            Assert.Equal((ushort)PacketOpcode.WorldAuthenticate, opcode);

            var serializer = new BinarySerializer();
            var packet = await serializer.DeserializeAsync<WorldAuthenticatePacket>(remaining);

            Assert.Equal(WorldId, packet.WorldId);

            Assert.Equal(Account, packet.Account.Text);
            Assert.Equal(Account.Length, packet.Account.Length);

            Assert.Equal(Password, packet.Password.Text);
            Assert.Equal(Password.Length, packet.Password.Length);
        }
    }
}
