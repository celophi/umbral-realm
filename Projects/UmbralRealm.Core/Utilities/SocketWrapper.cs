using System.Net;
using System.Net.Sockets;

namespace UmbralRealm.Core.Utilities
{
    /// <summary>
    /// Provides access to an underlying <see cref="Socket"/>
    /// </summary>
    public class SocketWrapper : IDisposable
    {
        /// <summary>
        /// Underlying concrete socket instance.
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// Creates a wrapper around a <see cref="Socket"/> mostly for unit testing.
        /// </summary>
        /// <param name="socket"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SocketWrapper(Socket socket)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
        }

        /// <summary>
        /// Establishes a connection to a remote host.
        /// </summary>
        /// <param name="remoteEP">An System.Net.EndPoint that represents the remote device.</param>
        public virtual void Connect(EndPoint remoteEP) => _socket.Connect(remoteEP);

        /// <summary>
        /// Sets the specified System.Net.Sockets.Socket option to the specified System.Boolean value.
        /// </summary>
        /// <param name="optionLevel">One of the System.Net.Sockets.SocketOptionLevel values.</param>
        /// <param name="optionName">One of the System.Net.Sockets.SocketOptionName values.</param>
        /// <param name="optionValue">The value of the option, represented as a System.Boolean.</param>
        public virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue) =>
            _socket.SetSocketOption(optionLevel, optionName, optionValue);

        /// <summary>
        /// Associates a System.Net.Sockets.Socket with a local endpoint.
        /// </summary>
        /// <param name="localEP">The local System.Net.EndPoint to associate with the System.Net.Sockets.Socket.</param>
        public virtual void Bind(EndPoint localEP) => _socket.Bind(localEP);

        /// <summary>
        /// Places a System.Net.Sockets.Socket in a listening state.
        /// </summary>
        public virtual void Listen() => _socket.Listen();

        /// <summary>
        /// Accepts an incoming connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual ValueTask<Socket> AcceptAsync(CancellationToken cancellationToken) => _socket.AcceptAsync(cancellationToken);

        /// <summary>
        /// Sends data to a connected socket.
        /// </summary>
        /// <param name="buffer">The buffer for the sent data.</param>
        /// <param name="socketFlags">A bitwise combination of SocketFlags values that will be used when sending the data.</param>
        /// <returns></returns>
        public virtual async Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags socketFlags) =>
            await _socket.SendAsync(buffer, socketFlags);

        /// <summary>
        /// Receives data from a connected socket.
        /// </summary>
        /// <param name="buffer">The buffer for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of SocketFlags values that will be used when receiving the data.</param>
        /// <returns></returns>
        public virtual async Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags socketFlags) =>
            await _socket.ReceiveAsync(buffer, socketFlags);

        /// <summary>
        /// Sets low level operating modes for the socket.
        /// </summary>
        /// <param name="ioControlCode"></param>
        /// <param name="optionInValue"></param>
        /// <param name="optionOutValue"></param>
        /// <returns></returns>
        public virtual int IOControl(IOControlCode ioControlCode, byte[]? optionInValue, byte[]? optionOutValue) =>
            _socket.IOControl(ioControlCode, optionInValue, optionOutValue);

        /// <summary>
        /// Disables sends and receives on a System.Net.Sockets.Socket.
        /// </summary>
        /// <param name="how">One of the System.Net.Sockets.SocketShutdown values that specifies the operation that will no longer be allowed.</param>
        public virtual void Shutdown(SocketShutdown how) => _socket.Shutdown(how);

        /// <summary>
        /// Closes the socket connection and allows reuse of the socket.
        /// </summary>
        /// <param name="reuseSocket">true if this socket can be reused after the current connection is closed; otherwise, false</param>
        public virtual void Disconnect(bool reuseSocket) => _socket.Disconnect(reuseSocket);

        /// <summary>
        /// Closes the System.Net.Sockets.Socket connection and releases all associated resources.
        /// </summary>
        public virtual void Close() => _socket.Close();

        /// <summary>
        /// Disposes the underlying socket.
        /// </summary>
        public virtual void Dispose()
        {
            _socket.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
