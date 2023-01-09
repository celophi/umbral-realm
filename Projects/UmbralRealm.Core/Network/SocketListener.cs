using Microsoft.Extensions.Hosting;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Utilities;
using UmbralRealm.Core.Utilities.Interfaces;
using UmbralRealm.Proxy;

namespace UmbralRealm.Core.Network
{
    public class SocketListener : BackgroundService
    {
        /// <summary>
        /// Used for creating sockets.
        /// </summary>
        private readonly ISocketFactory _socketFactory;

        /// <summary>
        /// Used to hand off newly accepted connections.
        /// </summary>
        private readonly IDataMediator<ISocketConnection> _connectionMediator;

        /// <summary>
        /// Socket instance for accepting client connections.
        /// </summary>
        private SocketWrapper? _listener;

        /// <summary>
        /// Creates a TCP server that can accept client connections.
        /// </summary>
        /// <param name="socketFactory"></param>
        /// <param name="connectionMediator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SocketListener(ISocketFactory socketFactory, IDataMediator<ISocketConnection> connectionMediator)
        {
            _socketFactory = socketFactory ?? throw new ArgumentNullException(nameof(socketFactory));
            _connectionMediator = connectionMediator ?? throw new ArgumentNullException(nameof(connectionMediator));
        }

        /// <summary>
        /// Starts the server and prepares the listening socket and client queue.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener = _socketFactory.CreateListeningSocket();
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
            _listener?.Close();
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Accepts incoming connections and places them on a queue to be processed.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && _listener != null)
            {
                var socket = await _listener.AcceptAsync(stoppingToken);
                if (socket == null)
                {
                    await Task.Delay(1, stoppingToken);
                    continue;
                }

                var socketAdapter = new SocketWrapper(socket);
                var socketConnection = new SocketConnection(socketAdapter);
                await _connectionMediator.Publish(socketConnection);
            }
        }
    }
}
