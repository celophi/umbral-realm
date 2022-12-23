namespace UmbralRealm.Core.Network.Interfaces
{
    public interface ISocketConnection
    {
        /// <summary>
        /// Returns 'true' if the underlying socket is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Sends bytes to the remote connected to the socket.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        Task<int> SendAsync(byte[] buffer);

        /// <summary>
        /// Receives bytes read from the socket.
        /// </summary>
        /// <param name="size">Optional. Specifies a determined size for the buffer.</param>
        /// <returns></returns>
        Task<byte[]> ReceiveAsync(int? size = null);

        /// <summary>
        /// Sets TCP keep alive settings for the socket.
        /// </summary>
        void SetKeepAlive();

        /// <summary>
        /// Disconnect the socket and dispose the underlying resources.
        /// </summary>
        /// <returns></returns>
        void Disconnect();
    }
}
