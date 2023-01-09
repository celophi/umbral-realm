using System.Net;
using System.Net.Sockets;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Core.Network
{
    /// <summary>
    /// Factory for creating sockets for specific purposes.
    /// </summary>
    public class SocketFactory : ISocketFactory
    {
        /// <summary>
        /// IP address and port combination.
        /// </summary>
        private readonly IPEndPoint _endPoint;

        /// <summary>
        /// Creates a factory that can create instances of wrapped sockets.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SocketFactory(IPEndPoint endpoint)
        {
            _endPoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        /// <inheritdoc/>
        public SocketWrapper CreateListeningSocket()
        {
            var socket = this.CreateDefaultSocket();
            socket.Bind(_endPoint);
            socket.Listen();
            return socket;
        }

        /// <inheritdoc/>
        public SocketWrapper CreateConnectedSocket()
        {
            var socket = this.CreateDefaultSocket();
            socket.Connect(_endPoint);
            return socket;
        }

        private SocketWrapper CreateDefaultSocket() =>
            new(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
    }
}
