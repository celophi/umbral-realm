using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security;

namespace UmbralRealm.Proxy
{
    public class SocketClient
    {
        /// <summary>
        /// Server connection to read packets from and forward to the client.
        /// </summary>
        private IReadWriteConnection _server;

        /// <summary>
        /// Client connection to read packets from and forward to the server.
        /// </summary>
        private IWriteConnection _client;



        private readonly SocketWrapperFactory _socketFactory;

        private readonly IPEndPoint _endpoint;


        private BufferBlock<IWriteConnection> _clientTransmitQueue = new();
        private BufferBlock<IReadWriteConnection> _serverTransmitQueue = new();

        private BufferBlock<IPacket> _packetQueue;

        private IConnectionFactory _connectionFactory;

        public SocketClient(SocketWrapperFactory socketFactory, IPEndPoint endpoint, BufferBlock<IWriteConnection> requestQueue, IConnectionFactory connectionFactory)
        {
            _socketFactory = socketFactory ?? throw new ArgumentNullException(nameof(socketFactory));
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

            _packetQueue = new BufferBlock<IPacket>();


            requestQueue.LinkTo(this.CreateActionBlock<IWriteConnection>(this.HandleConnection));
            
            _clientTransmitQueue.LinkTo(this.CreateActionBlock<IWriteConnection>(this.TransmitClientPackets));
            _serverTransmitQueue.LinkTo(this.CreateActionBlock<IReadWriteConnection>(this.TransmitServerPackets));
        }

        public async Task HandleConnection(IWriteConnection connection)
        {
            _client?.Disconnect();
            _server?.Disconnect();

            var server = _socketFactory.Create();

            server.Connect(_endpoint);

            var remoteConnection = new SocketConnection(server);

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

            var serverConnection = _connectionFactory.Create(remoteConnection, cipher);

            _client = connection;
            _server = serverConnection;

            await _clientTransmitQueue.SendAsync(_client);
            await _serverTransmitQueue.SendAsync(_server);
        }

        public async Task TransmitClientPackets(IWriteConnection client)
        {
            if (_client?.IsConnected != true)
            {
                _server.Disconnect();
                return;
            }

            while ((_server?.IsConnected == true) && _client.TryGetPacket(out var packet))
            {
                if (packet is UnknownPacket unknownPacket)
                {
                    unknownPacket.Origin = PacketOrigin.Client;
                }

                if (this.PacketReceivedHandler != null)
                {
                    this.PacketReceivedHandler(packet, _client);
                }

                await _packetQueue.SendAsync(packet);
                await _server.SendAsync(packet);
            }

            await _clientTransmitQueue.SendAsync(_client);
        }

        public async Task TransmitServerPackets(IReadWriteConnection server)
        {
            if (_client?.IsConnected != true)
            {
                server.Disconnect();
                return;
            }

            if (server?.IsConnected == true)
            {
                await server.ReceiveAsync();
            }

            while (server?.TryGetPacket(out var packet) == true)
            {
                if (packet is UnknownPacket unknownPacket)
                {
                    unknownPacket.Origin = PacketOrigin.Server;
                }

                if (this.PacketReceivedHandler != null)
                {
                    this.PacketReceivedHandler(packet, _client);
                }

                await _packetQueue.SendAsync(packet);
                await _client.SendAsync(packet);
            }

            await _serverTransmitQueue.SendAsync(server);
        }

        /// <summary>
        /// Subscribes an observer to the connection buffer.
        /// </summary>
        /// <param name="observer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Subscribe(IObserver<IPacket> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            _packetQueue.AsObservable().Subscribe(observer);
        }

        /// <summary>
        /// Used for synchronously starting another proxy.
        /// </summary>
        public Action<IPacket, IWriteConnection> PacketReceivedHandler;

        /// <summary>
        /// Used for creating TPL dataflow action blocks from function and wrapping them in an exception handler.
        /// Dataflow blocks cannot be used if an exception happens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private ActionBlock<T> CreateActionBlock<T>(Func<T, Task> func)
        {
            try
            {
                return new ActionBlock<T>(func);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
