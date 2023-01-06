using System.Net.Sockets;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Proxy
{
    public class SocketConnection : ISocketConnection
    {
        /// <summary>
        /// Underlying socket to a client.
        /// </summary>
        private readonly SocketWrapper _socketAdapter;

        /// <summary>
        /// Size in bytes for each receive buffer.
        /// </summary>
        private const int BUFFER_SIZE = 8192;

        public SocketConnection(SocketWrapper socketAdapter)
        {
            _socketAdapter = socketAdapter ?? throw new ArgumentNullException(nameof(socketAdapter));
        }

        /// <inheritdoc/>
        public bool IsConnected { get; private set; } = true;

        /// <inheritdoc/>
        public async Task<int> SendAsync(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer));
            }

            try
            {
                return await _socketAdapter.SendAsync(buffer, SocketFlags.None);
            }
            catch (Exception)
            {
                this.Disconnect();
                return 0;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> ReceiveAsync(int? size = null)
        {
            size ??= BUFFER_SIZE;

            var bytesRead = 0;
            var buffer = new byte[(int)size];

            try
            {
                bytesRead = await _socketAdapter.ReceiveAsync(buffer, SocketFlags.None);
            }
            catch (Exception)
            {
                this.Disconnect();
            }

            if (bytesRead <=0)
            {
                this.Disconnect();
                return Array.Empty<byte>();
            }

            return buffer.Take(bytesRead).ToArray();
        }

        /// <inheritdoc/>
        public void SetKeepAlive()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new NotSupportedException();
            }

            using var writer = new BinaryStreamWriter();
            writer.PutUInt32(1);        // Turn Keep Alive On
            writer.PutUInt32(5000);     // Amount of time in ms of inactivity before packet is sent.
            writer.PutUInt32(5000);     // Amount of time between packet sends.

            _ = _socketAdapter.IOControl(IOControlCode.KeepAliveValues, writer.ToArray(), optionOutValue: null);
        }

        /// <inheritdoc/>
        public void Disconnect()
        {
            try
            {
                this.IsConnected = false;
                _socketAdapter.Shutdown(SocketShutdown.Both);
                _socketAdapter.Disconnect(reuseSocket: false);
                _socketAdapter.Close();
            }
            catch (Exception)
            {
                // It is expected that there will be exceptions when attempting to shutdown and disconnect sockets.
            }

            using (_socketAdapter) { }
        }
    }
}
