using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Login.Interfaces;
using UmbralRealm.Login.Packet.Client;

namespace UmbralRealm.Login
{
    public class LoginController
    {
        private readonly ILoginService _loginService;
        private readonly IServerInfoService _serverInfoService;

        public LoginController(ILoginService loginService, IServerInfoService serverInfoService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _serverInfoService = serverInfoService ?? throw new ArgumentNullException(nameof(serverInfoService));
        }

        public async Task LoginAuthenticate(IWriteConnection connection, LoginAuthenticatePacket packet)
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(packet);

            IPacket response = _loginService.BuildLoginRejectedPacket();

            var name = packet.Account.Text;

            if (_loginService.IsLoggedIn(name))
            {
                await connection.SendAsync(response);
                return;
            }

            if (!_loginService.DoesAccountExist(name))
            {
                await connection.SendAsync(response);
                return;
            }

            if (!_loginService.IsPasswordValid(name, packet.Password.Text))
            {
                await connection.SendAsync(response);
                return;
            }

            _loginService.Login(name);

            response = _serverInfoService.BuildWorldSelectionPacket();
            await connection.SendAsync(response);
        }
    }
}
