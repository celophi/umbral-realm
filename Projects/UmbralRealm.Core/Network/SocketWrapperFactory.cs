using System.Net;
using System.Net.Sockets;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Core.Network
{
    public class SocketWrapperFactory
    {
        private readonly IPEndPoint _endPoint;

        public SocketWrapperFactory(IPEndPoint endpoint)
        {
            _endPoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        public SocketWrapper CreateListeningSocket()
        {
            var socket = this.CreateDefaultSocket();
            socket.Bind(_endPoint);
            socket.Listen();
            return socket;
        }

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
