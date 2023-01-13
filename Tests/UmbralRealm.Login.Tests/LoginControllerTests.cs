using System;
using System.Threading.Tasks;
using Moq;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Login.Dto;
using UmbralRealm.Login.Interfaces;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;
using Xunit;

namespace UmbralRealm.Login.Tests
{
    public class LoginControllerTests
    {
        [Fact]
        public void LoginAuthenticate_ConstructorLoginServiceArgumentIsNull_Throws()
        {
            var serverInfoService = new Mock<IServerInfoService>();
            Assert.Throws<ArgumentNullException>(() => new LoginController(loginService: null!, serverInfoService.Object));
        }

        [Fact]
        public void LoginAuthenticate_ConstructorServerInfoServiceArgumentIsNull_Throws()
        {
            var loginService = new Mock<ILoginService>();
            var serverInfoService = new Mock<IServerInfoService>();
            Assert.Throws<ArgumentNullException>(() => new LoginController(loginService.Object, serverInfoService: null!));
        }

        [Fact]
        public async Task LoginAuthenticate_ConnectionIsNull_Throws()
        {
            var packet = new LoginAuthenticatePacket();
            var loginService = new Mock<ILoginService>();
            var serverInfoService = new Mock<IServerInfoService>();
            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.LoginAuthenticate(connection: null!, packet: packet));
        }

        [Fact]
        public async Task LoginAuthenticate_PacketIsNull_Throws()
        {
            var connection = new Mock<IWriteConnection>();
            var loginService = new Mock<ILoginService>();
            var serverInfoService = new Mock<IServerInfoService>();
            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.LoginAuthenticate(connection: connection.Object, packet: null!));
        }

        [Fact]
        public async Task LoginAuthenticate_AccountIsAlreadyLoggedIn_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                }
            };

            var response = new LoginRejectedPacket();
            var connection = new Mock<IWriteConnection>();

            var loginService = new Mock<ILoginService>();
            loginService.Setup(m => m.IsLoggedIn(packet.Account.Text)).Returns(true);
            loginService.Setup(m => m.BuildLoginRejectedPacket()).Returns(response);

            var serverInfoService = new Mock<IServerInfoService>();
            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await controller.LoginAuthenticate(connection.Object, packet);

            loginService.Verify(m => m.IsLoggedIn(packet.Account.Text), Times.Once);
            connection.Verify(m => m.SendAsync(response), Times.Once);
        }

        [Fact]
        public async Task LoginAuthenticate_AccountDoesNotExist_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                }
            };

            var response = new LoginRejectedPacket();
            var connection = new Mock<IWriteConnection>();

            var loginService = new Mock<ILoginService>();
            loginService.Setup(m => m.IsLoggedIn(packet.Account.Text)).Returns(false);
            loginService.Setup(m => m.DoesAccountExist(packet.Account.Text)).Returns(false);
            loginService.Setup(m => m.BuildLoginRejectedPacket()).Returns(response);

            var serverInfoService = new Mock<IServerInfoService>();
            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await controller.LoginAuthenticate(connection.Object, packet);

            loginService.Verify(m => m.DoesAccountExist(packet.Account.Text), Times.Once);
            connection.Verify(m => m.SendAsync(response), Times.Once);
        }

        [Fact]
        public async Task LoginAuthenticate_InvalidPassword_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                },
                Password = new Core.IO.LengthPrefixedString
                {
                    Text = "MyPassword"
                }
            };


            var response = new LoginRejectedPacket();
            var connection = new Mock<IWriteConnection>();

            var loginService = new Mock<ILoginService>();
            loginService.Setup(m => m.IsLoggedIn(packet.Account.Text)).Returns(false);
            loginService.Setup(m => m.DoesAccountExist(packet.Account.Text)).Returns(true);
            loginService.Setup(m => m.IsPasswordValid(packet.Account.Text, packet.Password.Text)).Returns(false);
            loginService.Setup(m => m.BuildLoginRejectedPacket()).Returns(response);

            var serverInfoService = new Mock<IServerInfoService>();
            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await controller.LoginAuthenticate(connection.Object, packet);

            loginService.Verify(m => m.IsPasswordValid(packet.Account.Text, packet.Password.Text), Times.Once);
            connection.Verify(m => m.SendAsync(response), Times.Once);
        }

        [Fact]
        public async Task LoginAuthenticate_ValidPassword_SendsLoginRejectedPacket()
        {
            var packet = new LoginAuthenticatePacket
            {
                Account = new Core.IO.LengthPrefixedString
                {
                    Text = "MyAccount"
                },
                Password = new Core.IO.LengthPrefixedString
                {
                    Text = "MyPassword"
                }
            };

            var response = new WorldSelectionPacket();
            var connection = new Mock<IWriteConnection>();

            var loginService = new Mock<ILoginService>();
            loginService.Setup(m => m.IsLoggedIn(packet.Account.Text)).Returns(false);
            loginService.Setup(m => m.DoesAccountExist(packet.Account.Text)).Returns(true);
            loginService.Setup(m => m.IsPasswordValid(packet.Account.Text, packet.Password.Text)).Returns(true);

            var serverInfoService = new Mock<IServerInfoService>();
            serverInfoService.Setup(m => m.BuildWorldSelectionPacket()).Returns(response);

            var controller = new LoginController(loginService.Object, serverInfoService.Object);

            await controller.LoginAuthenticate(connection.Object, packet);

            loginService.Verify(m => m.Login(packet.Account.Text), Times.Once);
            connection.Verify(m => m.SendAsync(response), Times.Once);
        }
    }
}
