using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Utilities;
using UmbralRealm.Core.Utilities.Interfaces;
using Xunit;

namespace UmbralRealm.Core.Tests.Network
{
    public class SocketListenerTests
    {
        [Fact]
        public void SocketFactoryArgumentNull_SocketListenerConstructed_Throws()
        {
            var mediator = new Mock<IDataMediator<ISocketConnection>>();
            Assert.Throws<ArgumentNullException>(() => new SocketListener(socketFactory: null!, connectionMediator: mediator.Object));
        }

        [Fact]
        public void ConnectionMediatorArgumentNull_SocketListenerConstructed_Throws()
        {
            var socketFactory = new Mock<ISocketFactory>();
            Assert.Throws<ArgumentNullException>(() => new SocketListener(socketFactory: socketFactory.Object, connectionMediator: null!));
        }

        [Fact]
        public async Task ArgumentsValid_SocketListenerStarted_ListeningSocketCreated()
        {
            var listeningSocket = new Mock<SocketWrapper>(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));

            var socketFactory = new Mock<ISocketFactory>();
            socketFactory.Setup(m => m.CreateListeningSocket()).Returns(listeningSocket.Object);

            var mediator = new Mock<IDataMediator<ISocketConnection>>();
            var listener = new SocketListener(socketFactory.Object, mediator.Object);

            await listener.StartAsync(default);

            socketFactory.Verify(m => m.CreateListeningSocket(), Times.Once);
        }

        [Fact]
        public async Task ArgumentsValid_SocketListenerStopped_ListeningSocketClosed()
        {
            var listeningSocket = new Mock<SocketWrapper>(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));

            var socketFactory = new Mock<ISocketFactory>();
            socketFactory.Setup(m => m.CreateListeningSocket()).Returns(listeningSocket.Object);

            var mediator = new Mock<IDataMediator<ISocketConnection>>();
            var listener = new SocketListener(socketFactory.Object, mediator.Object);

            await listener.StartAsync(default);
            await listener.StopAsync(default);

            listeningSocket.Verify(m => m.Close(), Times.Once);
        }

        [Fact]
        public async Task AcceptedConnections_SocketListenerRunning_ConnectionsPublishedToMediator()
        {
            var acceptedSocket = new Mock<Socket>(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var listeningSocket = new Mock<SocketWrapper>(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
            listeningSocket.Setup(m => m.AcceptAsync(It.IsAny<CancellationToken>())).Returns(new ValueTask<Socket>(acceptedSocket.Object));

            var socketFactory = new Mock<ISocketFactory>();
            socketFactory.Setup(m => m.CreateListeningSocket()).Returns(listeningSocket.Object);

            var mediator = new Mock<IDataMediator<ISocketConnection>>();
            var listener = new SocketListenerMock(socketFactory.Object, mediator.Object);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(100);

            await listener.StartAsync(cts.Token);

            listeningSocket.Verify(m => m.AcceptAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            mediator.Verify(m => m.Publish(It.IsAny<ISocketConnection>()), Times.AtLeastOnce);
        }

        private class SocketListenerMock : SocketListener
        {
            public SocketListenerMock(ISocketFactory socketFactory, IDataMediator<ISocketConnection> connectionMediator)
                : base(socketFactory, connectionMediator) { }

            public Task ExecuteAsyncProxy(CancellationToken stoppingToken) => base.ExecuteAsync(stoppingToken);
        }
    }
}
