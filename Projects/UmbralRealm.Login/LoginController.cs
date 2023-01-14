using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Domain.Models;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Interfaces;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login
{
    public class LoginController
    {
        private readonly ILoginService _loginService;
        private readonly IServerInfoService _serverInfoService;
        private readonly IAccountRepository _accountRepository;

        public LoginController(ILoginService loginService, IServerInfoService serverInfoService, IAccountRepository accountRepository)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _serverInfoService = serverInfoService ?? throw new ArgumentNullException(nameof(serverInfoService));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task LoginAuthenticate(IWriteConnection connection, LoginAuthenticatePacket packet)
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(packet);

            var validator = new LoginAuthenticatePacketValidator();
            var validationResult = await validator.ValidateAsync(packet);

            if (!validationResult.IsValid)
            {
                connection.Disconnect();
                return;
            }

            var username = new Username(packet.Account.Text);
            var accountEntity = await _accountRepository.GetByUsername(username);

            if (accountEntity == null)
            {
                await connection.SendAsync(new LoginRejectedPacket
                {
                    Reason = LoginFailureResult.InvalidCredentials1
                });
                return;
            }

            var account = new Account(accountEntity);
            var password = new MD5Hash(packet.Password.Text);
            
            if (account.Password != password)
            {
                var badresponse = new LoginRejectedPacket
                {
                    Reason = LoginFailureResult.InvalidCredentials1
                };
                await connection.SendAsync(badresponse);
                return;
            }

            var response = _serverInfoService.BuildWorldSelectionPacket();
            await connection.SendAsync(response);
        }
    }
}
