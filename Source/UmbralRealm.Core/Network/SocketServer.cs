using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Hosting;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities;
using UmbralRealm.Proxy;

namespace UmbralRealm.Core.Network
{
    public class SocketServer : BackgroundService
    {
        /// <summary>
        /// Socket instance for accepting client connections.
        /// </summary>
        private readonly SocketWrapper _listener;

        /// <summary>
        /// Socket endpoint for listening for connections.
        /// </summary>
        private readonly IPEndPoint _endPoint;

        /// <summary>
        /// Server RSA certificate for encrypting secrets and establishing a secure channel.
        /// </summary>
        private readonly NetworkCertificate _certificate;

        /// <summary>
        /// Used for creating connections.
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Buffer holding connections that need to be authenticated.
        /// </summary>
        private readonly BufferBlock<ISocketConnection> _unverifiedBuffer = new();

        /// <summary>
        /// Buffer holding connections that allows observers to subscribe.
        /// </summary>
        private readonly BufferBlock<IWriteConnection> _connectionServiceBuffer = new();

        /// <summary>
        /// Creates a TCP server that can accept client connections.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="endPoint"></param>
        /// <param name="certificate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SocketServer(SocketWrapper listener, IPEndPoint endPoint, NetworkCertificate certificate, IConnectionFactory connectionFactory)
        {
            _endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        /// <summary>
        /// Starts the server and prepares the listening socket and client queue.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Bind(_endPoint);
            _listener.Listen();

            var sendCertificateBlock = new TransformBlock<ISocketConnection?, ISocketConnection?>(async socketConnection => await this.SendCertificateAsync(socketConnection));
            var receiveSecretBlock = new TransformBlock<ISocketConnection?, IReadWriteConnection?>(async socketConnection => await this.ReceiveSecretAsync(socketConnection));
            var processRequestsBlock = new TransformBlock<IReadWriteConnection?, IReadWriteConnection?>(async connection => await this.ProcessRequestsAsync(connection));

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            _unverifiedBuffer.LinkTo(sendCertificateBlock, linkOptions, connection => connection != null);
            sendCertificateBlock.LinkTo(receiveSecretBlock, linkOptions, connection => connection != null);
            receiveSecretBlock.LinkTo(processRequestsBlock, linkOptions, connection => connection != null);
            processRequestsBlock.LinkTo(processRequestsBlock, linkOptions, connection => connection != null);

            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Stops the server and closes the listening socket.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Listening sockets do not need to disconnect or shutdown.
            _listener.Close();
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Accepts incoming connections and places them on a queue to be processed.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var socket = await _listener.AcceptAsync(stoppingToken);
                if (socket == null)
                {
                    await Task.Delay(1, stoppingToken);
                    continue;
                }

                var socketAdapter = new SocketWrapper(socket);
                var socketConnection = new SocketConnection(socketAdapter);
                socketConnection.SetKeepAlive();

                await _unverifiedBuffer.SendAsync(socketConnection);
            }
        }

        /// <summary>
        /// Subscribes an observer to the connection buffer.
        /// </summary>
        /// <param name="observer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Subscribe(IObserver<IWriteConnection> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            _connectionServiceBuffer.AsObservable().Subscribe(observer);
        }

        /// <summary>
        /// Sends the network certificate to each connected client for authentication.
        /// </summary>
        /// <param name="socketConnection"></param>
        /// <returns></returns>
        private async Task<ISocketConnection?> SendCertificateAsync(ISocketConnection? socketConnection)
        {
            try
            {
                if (socketConnection?.IsConnected != true)
                {
                    return null;
                }

                var packet = new RsaCertificatePacket();
                packet.Modulus = _certificate.GetRsaModulus();
                packet.Exponent = _certificate.GetRsaExponent();

                _ = await socketConnection.SendAsync(packet.Serialize());
                return socketConnection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Receives the generated client secret and creates a cipher that is used to encrypt/decrypt network traffic.
        /// </summary>
        /// <param name="socketConnection"></param>
        /// <returns></returns>
        private async Task<IReadWriteConnection?> ReceiveSecretAsync(ISocketConnection? socketConnection)
        {
            byte[] buffer;

            try
            {
                if (socketConnection?.IsConnected != true)
                {
                    return null;
                }

                buffer = await socketConnection.ReceiveAsync(256);

                if (!socketConnection.IsConnected || buffer.Length <= 0)
                {
                    return null;
                }

                var secret = _certificate.ProcessBlock(buffer);
                var cipher = NetworkCipher.Create(secret);

                var connection = _connectionFactory.Create(socketConnection, cipher);
                await _connectionServiceBuffer.SendAsync(connection);

                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Receives incoming buffered request data from each connnection in an ongoing loop.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task<IReadWriteConnection?> ProcessRequestsAsync(IReadWriteConnection? connection)
        {
            try
            {
                if (connection?.IsConnected != true)
                {
                    return null;
                }

                await connection.ReceiveAsync();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
