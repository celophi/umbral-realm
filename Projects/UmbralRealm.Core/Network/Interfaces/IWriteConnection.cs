using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Interfaces
{
    public interface IWriteConnection
    {
        /// <summary>
        /// Returns 'true' if the underlying socket is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Sends bytes from a buffer to a socket.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Byte array that was sent.</returns>
        Task<byte[]> SendAsync(IPacket packet);

        /// <summary>
        /// Attempts to retrieve an incoming request <see cref="IPacket"/> from the internal queue.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        bool TryGetPacket(out IPacket packet);

        /// <summary>
        /// Returns the network cipher key.
        /// </summary>
        byte[] GetCipherKey();

        /// <summary>
        /// Disconnect the socket and dispose the underlying resources.
        /// </summary>
        /// <returns></returns>
        void Disconnect();
    }
}
