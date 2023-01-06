using System.Collections.Concurrent;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Security;

namespace UmbralRealm.Core.Network
{
    public class Connection : IReadWriteConnection
    {
        private readonly ISocketConnection _connection;
        private readonly NetworkCipher _cipher;
        private readonly IPacketConverter _packetFactory;

        /// <summary>
        /// Uniquely identifies the connection.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Packet queue of requests to be read by the server.
        /// </summary>
        private readonly ConcurrentQueue<IPacket> _requests = new();

        public Connection(ISocketConnection connection, NetworkCipher cipher, IPacketConverter packetFactory)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _cipher = cipher ?? throw new ArgumentNullException(nameof(cipher));
            _packetFactory = packetFactory ?? throw new ArgumentNullException(nameof(packetFactory));

            this.Id = Guid.NewGuid();
        }

        /// <inheritdoc/>
        public bool IsConnected => _connection.IsConnected;

        /// <inheritdoc/>
        public bool TryGetPacket(out IPacket packet) => _requests.TryDequeue(out packet!);

        /// <inheritdoc/>
        public async Task<byte[]> SendAsync(IPacket packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            //var buffer = this.SerializePacket(packet);
            var buffer = _packetFactory.Serialize(packet, _cipher.Sending);
            _ = await _connection.SendAsync(buffer);
            return buffer;
        }

        /// <inheritdoc/>
        public async Task ReceiveAsync()
        {
            var buffer = await _connection.ReceiveAsync();

            if (!this.IsConnected || buffer.Length <= 0)
            {
                return;
            }

            var packets = _packetFactory.Deserialize(buffer, _cipher.Receiving);

            foreach (var packet in packets)
            {
                _requests.Enqueue(packet);
            }
        }

        /// <inheritdoc/>
        public byte[] GetCipherKey() => _cipher.Key;

        /// <inheritdoc/>
        public void Disconnect() => _connection.Disconnect();
    }
}
