using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Domain.Models;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Interfaces;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Service.Requests
{
    public class LoginAuthenticateRequestHandler : IRequestHandler<RequestContext<LoginAuthenticatePacket>, PipelineResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IServerInfoService _serverInfoService;

        public LoginAuthenticateRequestHandler(IAccountRepository accountRepository, IServerInfoService serverInfoService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _serverInfoService = serverInfoService ?? throw new ArgumentNullException(nameof(serverInfoService));
        }

        public async Task<PipelineResult> Handle(RequestContext<LoginAuthenticatePacket> requestContext, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(requestContext);

            var result = new PipelineResult();
            var request = requestContext.Request;
            var connection = requestContext.Connection;

            var username = new Username(request.Account.Text);
            var accountEntity = await _accountRepository.GetByUsername(username);

            if (accountEntity == null)
            {
                await this.SendInvalidCredentialsResponse(connection);
                return result;
            }

            var account = new Account(accountEntity);
            var password = new MD5Hash(request.Password.Text);

            if (account.Password != password)
            {
                await this.SendInvalidCredentialsResponse(connection);
                return result;
            }

            var response = _serverInfoService.BuildWorldSelectionPacket();
            await connection.SendAsync(response);

            return result;
        }

        private async Task SendInvalidCredentialsResponse(IWriteConnection connection)
        {
            await connection.SendAsync(new LoginRejectedPacket
            {
                Reason = LoginFailureResult.InvalidCredentials1
            });
        }
    }
}
