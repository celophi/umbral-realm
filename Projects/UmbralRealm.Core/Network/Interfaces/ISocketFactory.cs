using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Core.Network.Interfaces
{
    public interface ISocketFactory
    {
        /// <summary>
        /// Creates a socket that is listening on an endpoint.
        /// </summary>
        /// <returns></returns>
        public SocketWrapper CreateListeningSocket();

        /// <summary>
        /// Creates a socket that is connected to an endpoint.
        /// </summary>
        /// <returns></returns>
        public SocketWrapper CreateConnectedSocket();
    }
}
