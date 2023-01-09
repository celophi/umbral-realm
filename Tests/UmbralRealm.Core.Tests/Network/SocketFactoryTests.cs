using System;
using System.Net;
using System.Net.Sockets;
using Moq;
using Moq.Protected;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Utilities;
using Xunit;

namespace UmbralRealm.Core.Tests.Network
{
    public class SocketFactoryTests
    {
        [Fact]
        public void EndpointArgumentNull_SocketFactoryConstructed_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new SocketFactory(null!));
        }

        [Fact]
        public void EndpointArgumentValid_CreateListeningSocketInvoked_SocketIsBoundAndListening()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.2.3.4"), 0);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var socketWrapper = new Mock<SocketWrapper>(socket);
            var socketFactory = new Mock<SocketFactory>(endpoint);

            socketFactory.Protected().Setup<SocketWrapper>("CreateDefaultSocket").Returns(socketWrapper.Object);

            socketFactory.Object.CreateListeningSocket();

            socketWrapper.Verify(m => m.Bind(endpoint), Times.Once);
            socketWrapper.Verify(m => m.Listen(), Times.Once);
        }

        [Fact]
        public void EndpointArgumentValid_CreateConnectedSocketInvoked_SocketIsConnected()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.2.3.4"), 0);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var socketWrapper = new Mock<SocketWrapper>(socket);
            var socketFactory = new Mock<SocketFactory>(endpoint);

            socketFactory.Protected().Setup<SocketWrapper>("CreateDefaultSocket").Returns(socketWrapper.Object);

            socketFactory.Object.CreateConnectedSocket();

            socketWrapper.Verify(m => m.Connect(endpoint), Times.Once);
        }
    }
}
