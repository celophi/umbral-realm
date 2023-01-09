using System.Threading.Tasks.Dataflow;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities.Interfaces;

namespace UmbralRealm.Core.Network
{
    public class SocketServer : IDataSubscriber<ISocketConnection>
    {
        /// <summary>
        /// Server RSA certificate for encrypting secrets and establishing a secure channel.
        /// </summary>
        private readonly NetworkCertificate _certificate;

        /// <summary>
        /// Used for creating connections.
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Used for publishing validated connections to subscribers.
        /// </summary>
        private readonly IDataMediator<IWriteConnection> _connectionMediator;

        private readonly TransformBlock<ISocketConnection?, ISocketConnection?> _sendCertificateBlock;

        /// <summary>
        /// Creates a TCP server that can accept client connections.
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="connectionFactory"></param>
        /// <param name="connectionMediator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SocketServer(NetworkCertificate certificate, IConnectionFactory connectionFactory, IDataMediator<IWriteConnection> connectionMediator)
        {
            _certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _connectionMediator = connectionMediator ?? throw new ArgumentNullException(nameof(connectionMediator));

            _sendCertificateBlock = new TransformBlock<ISocketConnection?, ISocketConnection?>(async socketConnection => await this.SendCertificateAsync(socketConnection));
            var receiveSecretBlock = new TransformBlock<ISocketConnection?, IReadWriteConnection?>(async socketConnection => await this.ReceiveSecretAsync(socketConnection));
            var processRequestsBlock = new TransformBlock<IReadWriteConnection?, IReadWriteConnection?>(async connection => await this.ProcessRequestsAsync(connection));

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            _sendCertificateBlock.LinkTo(receiveSecretBlock, linkOptions, connection => connection != null);
            receiveSecretBlock.LinkTo(processRequestsBlock, linkOptions, connection => connection != null);
            processRequestsBlock.LinkTo(processRequestsBlock, linkOptions, connection => connection != null);
        }

        public async Task Handle(ISocketConnection data) => await _sendCertificateBlock.SendAsync(data);

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
                await _connectionMediator.Publish(connection);

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
