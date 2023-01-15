using System;
using System.Threading.Tasks;
using Moq;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;
using UmbralRealm.Login.Service.Interfaces;
using UmbralRealm.Login.Service.Requests;
using Xunit;

namespace UmbralRealm.Login.Service.Tests.Requests
{
    public class LoginAuthenticateRequestHandlerTests
    {
        [Fact]
        public void Constructor_AccountRespositoryArgumentIsNull_ThrowsArgumentNullException()
        {
            var serverInfoService = new Mock<IServerInfoService>();
            Assert.Throws<ArgumentNullException>(() => new LoginAuthenticateRequestHandler(accountRepository: null!, serverInfoService.Object));
        }

        [Fact]
        public void Constructor_ServerInfoServiceArgumentIsNull_ThrowsArgumentNullException()
        {
            var accountRepository = new Mock<IAccountRepository>();
            Assert.Throws<ArgumentNullException>(() => new LoginAuthenticateRequestHandler(accountRepository.Object, serverInfoService: null!));
        }

        [Fact]
        public async Task Handle_ContextIsNull_Throws()
        {
            var accountRepository = new Mock<IAccountRepository>();
            var serverInfoService = new Mock<IServerInfoService>();
            var handler = new LoginAuthenticateRequestHandler(accountRepository.Object, serverInfoService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(context: null!, default));
        }

        [Fact]
        public async Task Handle_AccountDoesNotExist_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                }
            };

            IPacket response = null;
            var connection = new Mock<IWriteConnection>();
            connection.Setup(m => m.SendAsync(It.IsAny<LoginRejectedPacket>()))
                .Callback<IPacket>(result => response = result);

            var accountRepository = new Mock<IAccountRepository>();
            var serverInfoService = new Mock<IServerInfoService>();
            var context = new RequestContext<LoginAuthenticatePacket>(connection.Object, packet);
            var handler = new LoginAuthenticateRequestHandler(accountRepository.Object, serverInfoService.Object);

            await handler.Handle(context, default);

            connection.Verify(m => m.SendAsync(It.IsAny<LoginRejectedPacket>()), Times.Once);
            Assert.Equal(LoginFailureResult.InvalidCredentials1, ((LoginRejectedPacket)response!).Reason);
        }

        [Fact]
        public async Task Handle_AccountPasswordInvalid_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                },
                Password = new Core.IO.LengthPrefixedString
                {
                    Text = "098f6bcd4621d373cade4e832627b4f7"
                }
            };

            var accountEntity = new Types.Entities.AccountEntity(1, packet.Account.Text, "098f6bcd4621d373cade4e832627b4f6", null);

            IPacket response = null;
            var connection = new Mock<IWriteConnection>();
            connection.Setup(m => m.SendAsync(It.IsAny<LoginRejectedPacket>()))
                .Callback<IPacket>(result => response = result);

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(m => m.GetByUsername(It.IsAny<Username>())).Returns(Task.FromResult(accountEntity));

            var serverInfoService = new Mock<IServerInfoService>();
            var context = new RequestContext<LoginAuthenticatePacket>(connection.Object, packet);
            var handler = new LoginAuthenticateRequestHandler(accountRepository.Object, serverInfoService.Object);

            await handler.Handle(context, default);

            connection.Verify(m => m.SendAsync(It.IsAny<LoginRejectedPacket>()), Times.Once);
            Assert.Equal(LoginFailureResult.InvalidCredentials1, ((LoginRejectedPacket)response!).Reason);
        }

        [Fact]
        public async Task Handle_AccountPasswordValid_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                },
                Password = new Core.IO.LengthPrefixedString
                {
                    Text = "098f6bcd4621d373cade4e832627b4f6"
                }
            };

            var accountEntity = new Types.Entities.AccountEntity(1, packet.Account.Text, "098f6bcd4621d373cade4e832627b4f6", null);

            var connection = new Mock<IWriteConnection>();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(m => m.GetByUsername(It.IsAny<Username>())).Returns(Task.FromResult(accountEntity));

            var serverInfoService = new Mock<IServerInfoService>();
            var context = new RequestContext<LoginAuthenticatePacket>(connection.Object, packet);
            var handler = new LoginAuthenticateRequestHandler(accountRepository.Object, serverInfoService.Object);

            await handler.Handle(context, default);

            connection.Verify(m => m.SendAsync(It.IsAny<WorldSelectionPacket>()), Times.Once);
        }
    }
}
