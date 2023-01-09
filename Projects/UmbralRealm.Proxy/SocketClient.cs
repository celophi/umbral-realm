using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities.Interfaces;

namespace UmbralRealm.Proxy
{
    public class SocketClient : IDataSubscriber<IWriteConnection>
    {
        private readonly SocketWrapperFactory _socketFactory;

        private IConnectionFactory _connectionFactory;

        private CancellationTokenSource? _cts;

        private readonly IDataMediator<IPacket> _packetMediator;

        public SocketClient(SocketWrapperFactory socketFactory, IConnectionFactory connectionFactory, IDataMediator<IPacket> packetMediator)
        {
            _socketFactory = socketFactory ?? throw new ArgumentNullException(nameof(socketFactory));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _packetMediator = packetMediator ?? throw new ArgumentNullException(nameof(packetMediator));
        }

        /// <summary>
        /// Used for synchronously starting another proxy.
        /// </summary>
        public Action<IPacket, IWriteConnection> PacketReceivedHandler;

        public async Task Handle(IWriteConnection connection)
        {
            _cts?.Cancel();
            _cts = new();

            var remoteConnection = new SocketConnection(_socketFactory.CreateConnectedSocket());

            // Receive the RSA public key.
            var buffer = await remoteConnection.ReceiveAsync(RsaCertificatePacket.FixedSize);

            // Build an RSA certificate packet from received data.
            using var reader = new BinaryStreamReader(buffer);
            var packet = new RsaCertificatePacket();
            packet.Deserialize(reader);

            // Store the public key for later encryption.
            var certificate = NetworkCertificate.CreatePublic(packet.Modulus, packet.Exponent);

            // Send the cipher key
            var cipherKey = connection.GetCipherKey();
            var payload = certificate.ProcessBlock(cipherKey);

            await remoteConnection.SendAsync(payload);

            var cipher = NetworkCipher.Create(cipherKey);
            var server = _connectionFactory.Create(remoteConnection, cipher);

            _ = Task.Run(async () => await this.Transmit(server, connection, PacketOrigin.Client, _cts.Token));
            _ = Task.Run(async () => await this.Transmit(server, connection, PacketOrigin.Server, _cts.Token));
        }

        private async Task Transmit(IReadWriteConnection server, IWriteConnection connection, PacketOrigin origin, CancellationToken cancellationToken)
        {
            var sender = origin == PacketOrigin.Server ? server : connection;
            var receiver = origin == PacketOrigin.Server ? connection : server;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (connection?.IsConnected != true)
                {
                    server.Disconnect();
                    return;
                }

                if (server?.IsConnected != true)
                {
                    connection.Disconnect();
                    return;
                }

                if (origin == PacketOrigin.Server)
                {
                    await server.ReceiveAsync();
                }

                while (sender.TryGetPacket(out var packet))
                {
                    if (packet is UnknownPacket unknownPacket)
                    {
                        unknownPacket.Origin = origin;
                    }

                    this.PacketReceivedHandler?.Invoke(packet, connection);

                    await _packetMediator.Publish(packet);
                    await receiver.SendAsync(packet);
                }
            }
        }
    }
}
